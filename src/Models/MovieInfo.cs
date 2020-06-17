using System;
using Topdev.OpenSubtitles.Client;

namespace Topdev.Sublee.Cli.Models
{
    public class MovieInfo : MediaInfo
    {
        public int Year => int.Parse(_sub.MovieYear);

        public MovieInfo(Subtitles sub, string filePath) 
            : base(sub, filePath)
        {
        }

        public override string ToString() => $"{Name} ({Year})";
    }
}