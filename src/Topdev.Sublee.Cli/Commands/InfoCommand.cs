using System.Text.Json;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Topdev.Sublee.Cli.Commands;
using Topdev.Sublee.Cli.Models;

[Command(Description = "Show info about media file")]
public class InfoCommand : BaseCommand
{
    private readonly MediaInfoFactory _mifactory;

    [Argument(0, "input", "File or directory to be renamed")]
    public string Input { get; }

    public InfoCommand(ILogger<BaseCommand> logger) : base(logger)
    {
        _mifactory = new MediaInfoFactory(_api, logger);
    }

    protected override void Execute(CommandLineApplication app)
    {
        var mediaInfo = _mifactory.Create(Input);
        
        var json = JsonSerializer.Serialize(mediaInfo, 
            mediaInfo.GetType(),
            new JsonSerializerOptions() { WriteIndented = true });

        System.Console.WriteLine(json);
    }
}