using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Topdev.OpenSubtitles.Client;

namespace Topdev.Sublee.Cli.Models
{
    public class MediaInfo
    {
        protected readonly Subtitles _sub;
        protected readonly string _filePath;

        public string FilePath => _filePath;
        public string Extension => Path.GetExtension(_filePath).Substring(1);
        public virtual string Name => _sub.MovieName;

        public MediaInfo(Subtitles sub, string filePath)
        {
            _sub = sub;
            _filePath = filePath;
        }

        public override string ToString() => _sub.MovieName;
    }
}