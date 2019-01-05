using System.IO;
using System.Linq;
using Topdev.OpenSubtitles;

namespace Topdev.Sublee.Cli
{
    public class Renamer
    {
        private OpenSubtitlesApi openSubtitlesApi = new OpenSubtitlesApi();
        public void Rename(string filepath, string outputPath, string extension)
        {
            openSubtitlesApi.LogIn("eng", "sublee");

            var subtitles = openSubtitlesApi.FindSubtitles(SearchMethod.MovieHash, filepath, "eng")
                .FirstOrDefault();

            if (subtitles != null)
            {
                // Build path for found subtitles
                var path = BuildOutputPath(subtitles, outputPath, extension);

                // Check if direcotry exists if not create
                new FileInfo(path).Directory.Create();

                // Move source file to new location
                File.Move(filepath, path);
            }
            else
            {
                System.Console.WriteLine("[ERROR] Cannot find file on opensubtitles.");
            }

        }

        private string BuildOutputPath(Subtitles sub, string rootPath, string extension)
        {
            var path = rootPath;

            switch (sub.MovieKind)
            {
                case "episode":
                    var nameComponents = sub.MovieName.Split("\"");
                    var serialName = nameComponents[1];
                    var episodeName = nameComponents[2].Substring(1);

                    var filename = string.Format("{0} - S{1:D2}E{2:D2} - {3}{4}", 
                        serialName, 
                        int.Parse(sub.SeriesSeason), 
                        int.Parse(sub.SeriesEpisode), 
                        episodeName,
                        extension);

                    path = Path.Combine(path, "TV Shows", serialName, $"Season {sub.SeriesSeason}", filename);
                    break;
                case "movie":
                    var movie = $"{sub.MovieName} ({sub.MovieYear})";
                    path = Path.Combine(path, "Movies", movie, $"{movie}{extension}");
                    break;
            }

            return path;
        }
    }
}