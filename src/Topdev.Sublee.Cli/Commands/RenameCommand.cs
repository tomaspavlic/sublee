using System;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Topdev.OpenSubtitles;
using Topdev.Sublee.Cli.Models;

namespace Topdev.Sublee.Cli.Commands
{
    public class RenameCommand : BaseCommand
    {
        private string[] _allowedExtensions;
        private readonly MediaInfoFactory _mifactory;
        private readonly IFilePathGenerator _filePathGenerator = new PlexFilePathGenerator();

        [Argument(0, "input", "File or directory to be renamed.")]
        public string Input { get; }

        [Option("-e|--extensions <ex1>|<ex2>|<ex3>", Description = "Allowed extensions of files to be renamed")]
        public string Extensions { get; } = "avi|mkv|mp4|mov";

        [Option("-o|--output <path>", Description = "Path for output directory")]
        public string Output { get; }

        [Option("-r|--replace", Description = "Be verbose")]
        public bool Replace { get; } = false;

        public RenameCommand(ILogger<RenameCommand> logger, OpenSubtitlesApi api)
            : base(logger)
        {
            _mifactory = new MediaInfoFactory(api, logger);
        }

        protected override void Execute(CommandLineApplication app)
        {
            _allowedExtensions = Extensions.Split('|').Select(e => $".{e}").ToArray();

            // if input is directory path
            if (Directory.Exists(Input))
            {
                var files = Directory.GetFiles(Input, "*", SearchOption.AllDirectories)
                    .Where(IsValidFile);

                foreach (var file in files)
                {
                    if (Verbose) _logger.LogInformation($"File found {file}.");
                    Rename(file);
                }
            }
            // if input is filepath
            else if (IsValidFile(Input))
            {
                Rename(Input);
            }
            else
            {
                throw new Exception("Path of input value does not exists");
            }
        }

        private void Rename(string file)
        {
            if (Verbose) _logger.LogInformation($"Retrieving information about file {file}.");
            // Retrieve file information
            var mediaInfo = _mifactory.Create(file);

            if (Verbose) _logger.LogInformation($"Identified as '{mediaInfo}'.");
            // Generate output file path
            var outputFilePath = _filePathGenerator.Generate(mediaInfo, Output);

            if (Verbose) _logger.LogInformation($"New path for file generated '{outputFilePath}'.");
            // Check if direcotry exists if not create
            new FileInfo(outputFilePath).Directory.Create();

            if (File.Exists(outputFilePath))
            {
                if (Replace)
                {
                    File.Delete(outputFilePath);
                }
                else
                {
                    _logger.LogWarning("File already exists. Skipping.");
                    return;
                }
            }

            if (Verbose) _logger.LogInformation($"Moving file to new location.");
            // Move file to newly generated path
            File.Move(mediaInfo.FilePath, outputFilePath);
        }

        private bool IsValidFile(string file)
            => File.Exists(file) && _allowedExtensions.Contains(Path.GetExtension(file));
    }
}