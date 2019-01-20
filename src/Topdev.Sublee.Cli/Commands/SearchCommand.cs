using System;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Topdev.OpenSubtitles;

namespace Topdev.Sublee.Cli.Commands
{
    public class SearchCommand : BaseCommand
    {
        private readonly OpenSubtitlesApi _api;

        [Argument(0, "search", "Search value depends on search method. moviehash <path>, query <text>, imdb <id>, tag <text>")]
        public string Search { get; }

        [AllowedValues("moviehash", "query", "tag", "imdbid", IgnoreCase = true)]
        [Option("-m|--method <method>", Description = "Search method moviehash, query, tag or imdb")]
        public SearchMethod Method { get; } = SearchMethod.MovieHash;

        [Option("-o|--output <path>", Description = "Path for output subtitles filename default is original substitles name")]
        public string Output { get; }

        [Option("-l|--language <lang>", Description = "Language of subtitles default is english (eng). ISO 639-2/B")]
        public string Language { get; } = "eng";

        [Option("-1|--first", Description = "Download first subtitles without user input")]
        public bool First { get; }

        public SearchCommand(ILogger<SearchCommand> logger, OpenSubtitlesApi api)
            : base(logger)
        {
            _api = api;
        }

        protected override void Execute(CommandLineApplication app)
        {
            int selectedIndex = 0;
            if (Verbose) _logger.LogInformation($"Searching for subtitles using method '{Method}' with argument '{Search}' for language '{Language}'.");
            var subtitles = _api.FindSubtitles(Method, Search, Language);

            if (subtitles.Length == 0)
                _logger.LogWarning($"No subtitles found.");

            if (!First)
            {
                var consoleList = new ConsoleList("What subtitles do you want to download",
                    subtitles.Select(x => x.SubFileName).ToArray());

                selectedIndex = consoleList.ReadResult();
            }

            var selectedSubtitles = subtitles[selectedIndex];
            var outputFilePath = BuildOutputFilePath(Output, selectedSubtitles.SubFileName);

            if (Verbose) _logger.LogInformation($"Downloading: '{selectedSubtitles.MovieName} ({selectedSubtitles.MovieYear})' to '{outputFilePath}'.");
            _api.DownloadSubtitle(selectedSubtitles, outputFilePath);

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