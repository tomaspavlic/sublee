using System;
using System.IO;

namespace Topdev.Sublee.Cli.Models
{
    public class PlexFilePathGenerator : IFilePathGenerator
    {
        public string Generate(MediaInfo mi, string rootPath)
        {
            switch(mi)
            {
                case MovieInfo m:
                    return Path.Combine(rootPath, "Movies", $"{m.Name} ({m.Year})", $"{m.Name}.{m.Extension}");
                case TVShowInfo s:
                    return Path.Combine(rootPath, 
                        "TV Shows", 
                        s.Name, 
                        $"Season {s.Season}", 
                        string.Format("{0} - S{1:D2}E{2:D2} - {3}.{4}", s.Name, s.Season, s.Episode, s.EpisodeName, s.Extension));
                default:
                    throw new Exception("Unknown type of media.");
            }
        }
    }
}