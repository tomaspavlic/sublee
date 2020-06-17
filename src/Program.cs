using System.IO;
using System;
using System.Linq;
using Topdev.OpenSubtitles.Client;
using System.Net.Http;
using McMaster.Extensions.CommandLineUtils;
using Topdev.Sublee.Cli.Commands;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.DependencyInjection;

namespace Topdev.Sublee.Cli
{
    [Command]
    [Subcommand(
        typeof(SearchCommand), 
        typeof(RenameCommand),
        typeof(InfoCommand))]
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await new HostBuilder()
                // .ConfigureLogging((context, builder) => {
                //     builder.AddConsole();})
                .RunCommandLineApplicationAsync<Program>(args);
        }

        private void OnExecute(CommandLineApplication app)
        {
            app.Name = "sublee";
            app.FullName = "Command line application for sorting media, searching and downloading subtitles from OpenSubtitles.org";
            app.ShowHelp();
        }
    }
}
