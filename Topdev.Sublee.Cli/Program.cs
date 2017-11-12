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
            var app = new CommandLineApplication();
            var search = app.Argument("search", "Search value depends on search method. moviehash <path>, query <text>, imdb <id>, tag <text>", false);

            var method = app.Option("-m|--method <method>", "Search method moviehash, query, tag or imdb.", CommandOptionType.SingleValue);
            var output = app.Option("-o|--output <path>", "Path for output subtitles filename default is original substitles name.", CommandOptionType.SingleValue);
            var language = app.Option("-l|--language <lang>", "Language of subtitles default is english (eng). ISO 639-2/B", CommandOptionType.SingleValue);
            var first = app.Option("-1|--first", "Download first subtitles without user input.", CommandOptionType.NoValue);
            var verbose = app.Option("-v|--verbose", "Be verbose.", CommandOptionType.NoValue);

            app.Name = "sublee";
            app.FullName = "Command line application for downloading and searching subtitles from OpenSubtitles.org";
            app.Syntax = "Sublee OpenSubtitles Command Line Interface (1.1.0)";
            app.HelpOption("-?|-h|--help");

            app.OnExecute(() =>
            {
                var openSubtitlesApi = new OpenSubtitlesApi();
                var searchMethod = ParseSearchMethod(method.Value());
                int selectedSubtitles = 0;
                string searchValue = search.Value;
                string lang = (language.HasValue() ? language.Value() : "eng");

                openSubtitlesApi.LogIn(lang, "sublee");

                var subtitles = openSubtitlesApi.FindSubtitles(searchMethod, searchValue, lang);

                if (subtitles.Length == 0)
                    return 0;

                if (!first.HasValue())
                {
                    var consoleList = new ConsoleList(
                        "What subtitles do you want to download", 
                        subtitles.Select(x => x.SubFileName).ToArray());

                    selectedSubtitles = consoleList.ReadResult();
                }

                openSubtitlesApi.DownloadSubtitle(
                    subtitles[selectedSubtitles],
                    output.HasValue() ? output.Value() : null);

                return 0;
            });

            app.Execute(args);
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
    }
}
