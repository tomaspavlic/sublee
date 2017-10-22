using Microsoft.Extensions.CommandLineUtils;
using System.IO;
using System;
using System.Linq;
using Topdev.OpenSubtitles;

namespace Topdev.Sublee.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineApplication app = new CommandLineApplication();
            CommandArgument search = app.Argument("search", "Search value depends on search method. moviehash <path>, query <text>, imdb <id>, tag <text>", false);

            CommandOption method = app.Option("-m|--method <method>", "Search method moviehash, query, tag or imdb.", CommandOptionType.SingleValue);
            CommandOption output = app.Option("-o|--output <path>", "Path for output subtitles filename default is original substitles name.", CommandOptionType.SingleValue);
            CommandOption language = app.Option("-l|--language <lang>", "Language of subtitles default is english (eng). ISO 639-2/B", CommandOptionType.SingleValue);
            CommandOption first = app.Option("-1|--first", "Download first subtitles without user input.", CommandOptionType.NoValue);
            CommandOption verbose = app.Option("-v|--verbose", "Be verbose.", CommandOptionType.NoValue);

            app.Name = "sublee";
            app.FullName = "Command line application for downloading and searching subtitles from OpenSubtitles.org";
            app.Syntax = "Sublee OpenSubtitles Command Line Interface (1.0.0)";
            app.HelpOption("-?|-h|--help");

            app.OnExecute(() =>
            {
                var openSubtitlesApi = new OpenSubtitlesApi();
                var searchMethod = ParseSearchMethod(method.Value());
                int selectedSubtitles = 0;
                string searchValue = search.Value;

                openSubtitlesApi.LogIn("cz", "OSTestUserAgentTemp");

                Subtitles[] subtitles = openSubtitlesApi.FindSubtitles(
                    searchMethod,
                    searchValue,
                    (language.HasValue() ? language.Value() : "eng"));

                if (subtitles.Length == 0)
                    return 0;

                if (!first.HasValue())
                {
                    PrintSubtitles(subtitles);
                    selectedSubtitles = GetUserSelection(subtitles.Length) - 1;
                }

                openSubtitlesApi.DownloadSubtitle(
                    subtitles[selectedSubtitles],
                    output.HasValue() ? output.Value() : null);

                return 0;
            });

            app.Execute(args);
        }

        /// <summary>
        /// Write to console all subtitles with coresponding indexes.
        /// </summary>
        /// <param name="subtitles">Subtitles to be printer.</param>
        public static void PrintSubtitles(Subtitles[] subtitles)
        {
            for (int i = 0; i < subtitles.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {subtitles[i].SubFileName}");
            }
        }

        /// <summary>
        /// Convert search method from string into enum.
        /// </summary>
        /// <param name="method">Search method in string format.</param>
        /// <returns>Search method for OpenSubtitle api.</returns>
        /// <exception cref="Exception"></exception>
        private static SearchMethod ParseSearchMethod(string method)
        {
            switch (method)
            {
                case "moviehash": return SearchMethod.MovieHash;
                case "tag": return SearchMethod.Tag;
                case "query": return SearchMethod.Query;
                case "imdb": return SearchMethod.IMDBId;
            }

            throw new Exception("Search method does not exists.");
        }


        /// <summary>
        /// Asks user to type in integer in range 1 - (max).
        /// </summary>
        /// <param name="max">Maximum value for input range.</param>
        /// <returns>Validated user's input.</returns>
        private static int GetUserSelection(int max)
        {
            int index;

            while (true)
            {
                Console.Write("Select: ");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out index))
                {
                    if (index <= 0)
                        continue;

                    if (index > max)
                        continue;

                    return index;
                }

                Console.Write(Environment.NewLine);
            }
        }
    }
}
