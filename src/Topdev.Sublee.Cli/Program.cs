using System.IO;
using System;
using System.Linq;
using Topdev.OpenSubtitles;
using System.Net.Http;
using McMaster.Extensions.CommandLineUtils;
using Topdev.Sublee.Cli.Commands;

namespace Topdev.Sublee.Cli
{
    [Command]
    [Subcommand(typeof(SearchCommand), typeof(RenameCommand))]
    class Program
    {
        public static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        private void OnExecute(CommandLineApplication app)
        {
            app.Name = "sublee";
            app.FullName = "Command line application for downloading and searching subtitles from OpenSubtitles.org";
            app.ShowHelp();
        }
    }
}
