using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Topdev.OpenSubtitles;

namespace Topdev.Sublee.Cli.Commands
{
    public abstract class BaseCommand
    {
        protected CommandLineApplication _app;
        protected readonly ILogger<BaseCommand> _logger;
        protected readonly OpenSubtitlesApi _api = new OpenSubtitlesApi();

        [Option("-v|--verbose", Description = "Be verbose.")]
        public bool Verbose { get; } = false;
        protected virtual bool ShowHelp
            => _app.Options.All(o => o.Values.Count == 0) && _app.Arguments.All(a => a.Values.Count == 0);

        public BaseCommand(ILogger<BaseCommand> logger)
        {
            _logger = logger;
            try
            {
                _api.LogIn("eng", "sublee");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Environment.Exit(1);
            }
        }

        public void OnExecute(CommandLineApplication app)
        {
            _app = app;

            if (ShowHelp)
            {
                app.ShowHint();
                return;
            }

            try
            {
                Execute(app);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }
        protected abstract void Execute(CommandLineApplication app);
    }
}