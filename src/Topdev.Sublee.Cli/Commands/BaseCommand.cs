using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Topdev.Sublee.Cli.Commands
{
    public abstract class BaseCommand
    {
        protected CommandLineApplication _app;
        protected readonly ILogger<BaseCommand> _logger;

        [Option("-v|--verbose", Description = "Be verbose")]
        public bool Verbose { get; } = false;

        public BaseCommand(ILogger<BaseCommand> logger)
        {
            _logger = logger;
        }

        public void OnExecute(CommandLineApplication app)
        {
            _app = app;

            if (ShowHelp)
            {
                app.ShowHelp();
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
        protected virtual bool ShowHelp
            => _app.Options.All(o => o.Values.Count == 0) && _app.Arguments.All(a => a.Values.Count == 0);
        protected abstract void Execute(CommandLineApplication app);
    }
}