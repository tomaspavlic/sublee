using System;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Topdev.OpenSubtitles.Client;
using Topdev.Sublee.Cli.Extensions;

namespace Topdev.Sublee.Cli.Commands
{
    [Command(
        Description = "Search subtitles for given media file")]
    public class SearchCommand : BaseCommand
    {
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

        public SearchCommand(ILogger<SearchCommand> logger)
            : base(logger)
        {
            
        }

        protected override void Execute(CommandLineApplication app)
        {
            int selectedIndex = 0;
            if (Verbose) _logger.LogInformation($"Searching for subtitles using method '{Method}' with argument '{Search}' for language '{Language}'.");
            var subtitles = _api.FindSubtitles(Method, Search, Language);

            if (subtitles.Length == 0)
            {
                OutputExtensions.WriteError($"No subtitles found.");
                Environment.Exit(0);
            }

            if (!First)
            {
                var consoleList = new ConsoleList("What subtitles do you want to download",
                    subtitles.Select(x => x.SubFileName).ToArray());

                selectedIndex = consoleList.ReadResult();
            }

            var selectedSubtitles = subtitles[selectedIndex];
            var outputFilePath = BuildOutputFilePath(Output, selectedSubtitles.SubFileName);

            if (Verbose) OutputExtensions.WriteVerbose($"Downloading: '{selectedSubtitles.MovieName} ({selectedSubtitles.MovieYear})' to '{outputFilePath}'.");
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