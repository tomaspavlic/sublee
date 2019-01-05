using System;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Topdev.OpenSubtitles;

namespace Topdev.Sublee.Cli.Commands
{
    [Command("rename")]
    public class RenameCommand
    {
        private string[] _allowedExtensions;
        private readonly Renamer _renamer = new Renamer();

        [Argument(0, "input", "File or directory to be renamed.")]
        public string Input { get; }

        [Option("-e|--extensions <ex1>|<ex2>|<ex3>", Description = "Allowed extensions of files to be renamed")]
        public string Extensions { get; } = "avi|mkv|mp4|mov";

        [Option("-o|--output <path>", Description = "Path for output directory")]
        public string Output { get; }

        private void OnExecute(CommandLineApplication app)
        {
            _allowedExtensions = Extensions.Split("|").Select(e => $".{e}").ToArray();

            // if input is directory path
            if (Directory.Exists(Input))
            {
                var files = Directory.GetFiles(Input, "*", SearchOption.AllDirectories)
                    .Where(IsValidFile);

                foreach (var file in files)
                {
                    _renamer.Rename(file, Output, Path.GetExtension(file));
                }
            }
            // if input is filepath
            else if (IsValidFile(Input))
            { 
                _renamer.Rename(Input, Output, Path.GetExtension(Input));
            }
            else
            {
                System.Console.WriteLine("[ERROR] Path of input value does not exists.");
            }
        }

        private bool IsValidFile(string file)
            => File.Exists(file) && _allowedExtensions.Contains(Path.GetExtension(file));
    }
}