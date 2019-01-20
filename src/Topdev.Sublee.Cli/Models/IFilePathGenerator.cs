using System.IO;

namespace Topdev.Sublee.Cli.Models
{
    public interface IFilePathGenerator
    {
        string Generate(MediaInfo mi, string rootPath);
    }
}