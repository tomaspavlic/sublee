using System;
using System.IO;
using System.Linq;

namespace Topdev.Sublee.Cli.Models
{
    public class PlexFilePathGenerator : IFilePathGenerator
    {
        public string Generate(MediaInfo mi, string rootPath)
        {
            switch(mi)
            {
                case MovieInfo m:
                    var movieName = ReplaceInvalidPathChars(m.Name);
                    return Path.Combine(rootPath, "Movies", $"{movieName} ({m.Year})", $"{movieName} ({m.Year}).{m.Extension}");
                case TVShowInfo s:
                    var tvShowName = ReplaceInvalidPathChars(s.Name);
                    return Path.Combine(rootPath, 
                        "TV Shows", 
                        tvShowName, 
                        $"Season {s.Season}", 
                        string.Format("{0} - S{1:D2}E{2:D2} - {3}.{4}", tvShowName, s.Season, s.Episode, s.EpisodeName, s.Extension));
                default:
                    throw new Exception("Unknown type of media.");
            }
        }

        private string ReplaceInvalidPathChars(string name)
        {
            var pathInvalidChars = Path.GetInvalidFileNameChars()
                .Concat(Path.GetInvalidPathChars());

            foreach(var c in pathInvalidChars)
                name = name.Replace(c.ToString(), string.Empty);

            return name;
        }
    }
}