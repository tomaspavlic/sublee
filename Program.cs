using OpenSubtitles.Model;
using Microsoft.Extensions.CommandLineUtils;
using System.IO;
using System;
using System.Linq;

namespace OpenSubtitles
{
    class Program
    {
        private static bool _verbose;
        private static OpenSubtitlesClient _openSubtitlesClient;

        static void Main(string[] args)
        {
            CommandLineApplication app = new CommandLineApplication();
            _openSubtitlesClient = new OpenSubtitlesClient("http://api.opensubtitles.org:80/xml-rpc");

            _verbose = args.Any(a => a == "-v" || a == "--verbose");

            try
            {
                // Commands configurations
                app.Command("download", DownloadSubtitleConfiguration);
                app.Command("search", SearchAndDownloadSubtitleConfiguration);

                app.HelpOption("-? | -h | --help");
                app.Execute(args);
            }
            catch (Exception e)
            {
                Log(e.Message, LogLevel.ERR);
            }
        }

        /// <summary>
        /// Configuration and execution for command 'search'
        /// Download subtitles for given file or for all files in given directory using moviehasfh search method.
        /// </summary>
        /// <param name="target"></param>
        private static void SearchAndDownloadSubtitleConfiguration(CommandLineApplication target)
        {
            target.HelpOption("-? | -h | --help");
            CommandArgument file = target.Argument("file", "Enter filepath to movie", false);
            CommandOption searchMethod = target.Option("-m | --method", "Subtitle search method", CommandOptionType.SingleValue);
            CommandOption languageArgument = target.Option("-l | --language", "Language of subtitles", CommandOptionType.SingleValue);
            target.Option("-v | --verbose", "Be verbose", CommandOptionType.NoValue);

            target.OnExecute(() =>
            {
                Log("Logging into OpenSubtitles api.", LogLevel.INFO);
                _openSubtitlesClient.LogIn("cz", "OSTestUserAgentTemp");

                SearchSubtitlesMethod method = ParseSearchMethod(searchMethod.HasValue() ? searchMethod.Value() : "moviehash");
                Log($"Searching subtitles for '{file.Value}' using method {method.ToString().ToLower()}.", LogLevel.INFO);
                SearchSubtitles[] subtitles = _openSubtitlesClient.FindSubtitles(method, file.Value, languageArgument.HasValue() ? languageArgument.Value() : "cze");

                if (subtitles.Length == 0)
                {
                    Log("No subtitles found.", LogLevel.WARN);
                    return 0;
                }

                Console.WriteLine("=== FOUND SUBTITLES ===");
                for (int i = 0; i < subtitles.Length; i++)
                {
                    Console.WriteLine($"[{i + 1}] - {subtitles[i].SubFileName}");
                }
                
                int index = GetUserSelectionInt(subtitles.Length);

                Log($"Downloading subtitles for '{file.Value}'.", LogLevel.INFO);

                string subtitlesFilePath = null;
                SearchSubtitles foundSubtitle = subtitles[index - 1];

                if (method == SearchSubtitlesMethod.MovieHash)
                    subtitlesFilePath = Path.ChangeExtension(file.Value, foundSubtitle.SubFormat);

                _openSubtitlesClient.DownloadSubtitle(foundSubtitle, subtitlesFilePath);
                Log($"Subtitles successfully downloaded.", LogLevel.INFO);
                return 0;
            });
        }
        
        /// <summary>
        /// Configuration and execution for 'download' command.
        /// </summary>
        /// <param name="target">Target command.</param>
        private static void DownloadSubtitleConfiguration(CommandLineApplication target)
        {
            target.HelpOption("-? | -h | --help");
            CommandArgument fileArgument = target.Argument("file", "Enter filepath to movie.", false);
            CommandOption languageOption = target.Option("-l | --language", "Language of subtitles", CommandOptionType.SingleValue);
            string language = languageOption.HasValue() ? languageOption.Value() : "cze";
            target.Option("-v | --verbose", "Be verbose", CommandOptionType.NoValue);

            target.OnExecute(() =>
            {
                Log("Logging into OpenSubtitles api.", LogLevel.INFO);
                _openSubtitlesClient.LogIn("cz", "OSTestUserAgentTemp");

                FileAttributes attr = File.GetAttributes(fileArgument.Value);

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    string[] files = Directory.GetFiles(fileArgument.Value);

                    foreach (string file in files)
                    {
                        SearchAndDownload(SearchSubtitlesMethod.MovieHash, file, language);
                    }
                }
                else
                {
                    SearchAndDownload(SearchSubtitlesMethod.MovieHash, fileArgument.Value, language);
                }

                return 0;
            });
        }

        /// <summary>
        /// Search for subtitles and download it.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="file"></param>
        /// <param name="language"></param>
        private static void SearchAndDownload(SearchSubtitlesMethod method, string file, string language)
        {
            Log($"Searching subtitles for '{file}' using method moviehash.", LogLevel.INFO);
            SearchSubtitles subtitle = _openSubtitlesClient.FindSubtitles(SearchSubtitlesMethod.MovieHash, file, language).FirstOrDefault();

            if (subtitle != null)
            {
                Log($"Subtitles found '{subtitle.SubFileName}'.", LogLevel.INFO);
                Log($"Downloading subtitle for {file}.", LogLevel.INFO);
                string subtitlesFilePath = Path.ChangeExtension(file, subtitle.SubFormat);
                _openSubtitlesClient.DownloadSubtitle(subtitle, subtitlesFilePath);
                Log($"Subtitles successfully downloaded.", LogLevel.INFO);
            }
            else
            {
                Log($"Subtitles for '{file}' not found", LogLevel.WARN);
            }
        }

        /// <summary>
        /// Convert search method from string into enum.
        /// </summary>
        /// <param name="method">Search method in string format.</param>
        /// <returns>Search method for OpenSubtitle api.</returns>
        /// <exception cref="Exception"></exception>
        private static SearchSubtitlesMethod ParseSearchMethod(string method)
        {
            switch (method)
            {
                case "moviehash": return SearchSubtitlesMethod.MovieHash;
                case "tag": return SearchSubtitlesMethod.Tag;
                case "query": return SearchSubtitlesMethod.Query;
                case "imdb": return SearchSubtitlesMethod.IMDBId;
            }

            throw new Exception("Search method does not exists.");
        }

        /// <summary>
        /// Log information into console. Logs only if application received parameter -v|--verbose or log level is error or warning.
        /// </summary>
        /// <param name="logMessage">Log message</param>
        /// <param name="level">Log level.</param>
        private static void Log(string logMessage, LogLevel level)
        {
            if (_verbose || level == LogLevel.ERR || level == LogLevel.WARN)
            {
                switch (level)
                {
                    case LogLevel.ERR:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    case LogLevel.INFO:
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        break;
                    case LogLevel.WARN:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    default: break;
                }

                Console.WriteLine("[{0:yyyy-MM-dd hh:mm:ss}][{1}] {2}", DateTime.Now, level, logMessage);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Asks user to type in integer in range 1 - (max).
        /// </summary>
        /// <param name="max">Maximum value for input range.</param>
        /// <returns>Validated user's input.</returns>
        private static int GetUserSelectionInt(int max)
        {
            int index;
            
            while (true)
            {
                Console.Write("Select subtitles: ");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out index))
                {
                    if (index <= 0)
                    {
                        Log("Index can not be zero or less.", LogLevel.WARN); continue;
                    }

                    if (index > max)
                    {
                        Log("Index is out of range.", LogLevel.WARN); continue;
                    }

                    return index;
                }
                else
                {
                    Log("Input is not number.", LogLevel.WARN);
                }

                Console.Write(Environment.NewLine);
            }
        }

        private enum LogLevel
        {
            INFO,
            WARN,
            ERR
        }
    }
}
