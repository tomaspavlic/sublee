using System;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Topdev.OpenSubtitles;

namespace Topdev.Sublee.Cli
{
    [Command("search")]
    public class SearchCommand
    {
        [Argument(0, "search", "Search value depends on search method. moviehash <path>, query <text>, imdb <id>, tag <text>")]
        public string Search { get; }

        [AllowedValues("moviehash", "query", "tag", "imdbid", IgnoreCase = true)]
        [Option("-m|--method <method>", Description = "Search method moviehash, query, tag or imdb")]
        public SearchMethod Method { get; } = SearchMethod.MovieHash;

        [Option("-o|--output <path>", Description = "Path for output subtitles filename default is original substitles name")]
        public string Output { get; } = null;

        [Option("-l|--language <lang>", Description = "Language of subtitles default is english (eng). ISO 639-2/B")]
        public string Language { get; } = "eng";

        [Option("-1|--first", Description = "Download first subtitles without user input")]
        public bool First { get; }

        [Option("-v|--verbose", Description = "Be verbose")]
        public bool Verbose { get; } = false;

        private void OnExecute()
        {
            var openSubtitlesApi = new OpenSubtitlesApi();
            int selectedIndex = 0;

            if (Verbose) Console.WriteLine("[INFO] Logging into OpenSubtitles.org.");
            openSubtitlesApi.LogIn(Language, "sublee");

            try
            {
                if (Verbose) Console.WriteLine($"[INFO] Searching for subtitles using method '{Method}' with argument '{Search}' for language '{Language}'.");
                var subtitles = openSubtitlesApi.FindSubtitles(Method, Search, Language);

                if (subtitles.Length == 0)
                    Console.WriteLine($"[WARN] No subtitles found.");

                if (!First)
                {
                    var consoleList = new ConsoleList("What subtitles do you want to download",
                        subtitles.Select(x => x.SubFileName).ToArray());

                    selectedIndex = consoleList.ReadResult();
                }

                var selectedSubtitles = subtitles[selectedIndex];
                var outputFilePath = BuildOutputFilePath(Output, selectedSubtitles.SubFileName);

                if (Verbose) Console.WriteLine($"[INFO] Downloading: '{selectedSubtitles.MovieName} ({selectedSubtitles.MovieYear})' to '{outputFilePath}'.");
                openSubtitlesApi.DownloadSubtitle(selectedSubtitles, outputFilePath);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"[ERROR] {exception.Message}");
            }
        }

        /// <summary>
        /// Build path to output file. 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="subtitleFileName"></param>
        /// <returns></returns>
        private string BuildOutputFilePath(string output, string subtitleFileName)
        {
            if (string.IsNullOrEmpty(output))
                return Path.Combine(Environment.CurrentDirectory, subtitleFileName);

            if (Directory.Exists(output))
                return Path.Combine(output, subtitleFileName);

            return output;
        }
    }
}