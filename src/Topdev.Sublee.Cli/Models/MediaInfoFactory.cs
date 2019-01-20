using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Topdev.OpenSubtitles;

namespace Topdev.Sublee.Cli.Models
{
    public class MediaInfoFactory
    {
        private readonly OpenSubtitlesApi _api;
        private readonly ILogger _logger;
        public MediaInfoFactory(OpenSubtitlesApi api, ILogger logger)
        {
            _logger = logger;
            _api = api;
        }

        public MediaInfo Create(string filePath)
        {
            var subtitles = _api.FindSubtitles(SearchMethod.MovieHash, filePath, "eng")
                .GroupBy(s => s.MovieName)
                .Select(g => new { Subtitles = g.First(), Count = g.Count() })
                .OrderByDescending(s => s.Count)
                .Select(s => s.Subtitles)
                .FirstOrDefault();

            if (subtitles == null)
                throw new Exception("No information could be found.");
                
            if (subtitles.MovieKind == "episode")
            {
                return new TVShowInfo(subtitles, filePath);
            }
            else
            {
                return new MovieInfo(subtitles, filePath);
            }
        }
    }
}