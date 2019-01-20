using System.IO;
using System;
using System.Linq;
using Topdev.OpenSubtitles;
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
        typeof(RenameCommand))]
    class Program
    {
        // public static int Main(string[] args)
        //     => CommandLineApplication.Execute<Program>(args);

        public static async Task<int> Main(string[] args)
        {
            var api = new OpenSubtitlesApi();
            api.LogIn("eng", "sublee");

            return await new HostBuilder()
                .ConfigureLogging((context, builder) => {
                    builder.AddConsole();})
                .ConfigureServices((context, builder) => {
                    builder.AddSingleton(api);})
                .RunCommandLineApplicationAsync<Program>(args);
        }

        private void OnExecute(CommandLineApplication app)
        {
            app.Name = "sublee";
            app.FullName = "Command line application for downloading and searching subtitles from OpenSubtitles.org";
            app.ShowHelp();
        }
    }
}
