using SimpsonNostalgiaTV.Models;
using System.Text.RegularExpressions;

namespace SimpsonNostalgiaTV.Utils
{
    public static class EpisodeUtils
    {
        public static Episode GetRandomEpisode(string wwwrootPath, string serie)
        {
            Random rnd = new Random();
            var response = new Episode();
            var seriePath = Path.Combine(wwwrootPath, "storage", serie);

            try
            {
                List<string> seasonsPath = DirectoryUtils.GetSeasonsPaths(seriePath);
                List<List<string>> episodes = DirectoryUtils.GetEpisodesPaths(seasonsPath);

                //Get season and episode random
                int season = rnd.Next(episodes.Count);
                int episode = rnd.Next(episodes[season].Count);

                //EpisodePath
                var episodePath = episodes[season][episode];
                episodePath = Regex.Replace(episodePath, "[A-z0-0\\:]+wwwroot\\\\", "../"); //Remove absolute path
                episodePath = Regex.Replace(episodePath, "\\\\", "/"); //Remplace \\ to /

                //Get FileName
                FileInfo fileInfo = new FileInfo(episodePath);
                var fileName = Regex.Replace(fileInfo.Name, "[0-9]x[0-9]{2}", string.Empty).Trim(); // Remove episodeNum and seasonNum
                fileName = Regex.Replace(fileName, ".mp4", string.Empty); // Remove Format

                //Get File Duration
                var filePath = episodes[season][episode];
                var file = TagLib.File.Create(filePath);
                TimeSpan duration = file.Properties.Duration;

                //Get episode data
                response.Season = season;
                response.EpisodeNum = episode;
                response.Path = episodePath;
                response.Title = fileName;
                response.Duration = duration;
                response.DatePlay = DateTime.UtcNow;
                
            }
            catch(Exception ex)
            {
                
            }

            return response;
        }
    }
}
