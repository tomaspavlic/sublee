using System;
using Topdev.OpenSubtitles;

namespace Topdev.Sublee.Cli.Models
{
    public class TVShowInfo : MediaInfo
    {
        private readonly string[] _nameComponents;

        public int Season => int.Parse(_sub.SeriesSeason);
        public int Episode => int.Parse(_sub.SeriesEpisode);
        public string EpisodeName => _nameComponents[2].Substring(1);
        public override string Name => _nameComponents[1];

        public TVShowInfo(Subtitles sub, string filePath) 
            : base(sub, filePath)
        {
            _nameComponents = sub.MovieName.Split("\"");
        }

        public override string ToString() =>
            string.Format("{0} - S{1:D2}E{2:D2} - {3}", Name, Season, Episode, EpisodeName);
    }
}