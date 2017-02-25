using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace sublee.Extension
{
    public static class CommandLineApplicationExtension
    {
        public static void Command(this CommandLineApplication app, string name, Action<CommandLineApplication> configuration, Func<int> invoke)
        {
            app.Command(name, (target) =>
            {
                configuration(target);
                target.OnExecute(invoke);
            });
        }
    }
}
