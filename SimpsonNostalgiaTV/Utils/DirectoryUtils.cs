namespace SimpsonNostalgiaTV.Utils
{
    public static class DirectoryUtils
    {

        public static List<string> GetSeasonsPaths(string seriePath)
        {
            return Directory.GetDirectories(seriePath).ToList();
        }

        public static List<List<string>> GetEpisodesPaths(List<string> seasonsDirectories)
        {
            return seasonsDirectories.Select(x => Directory.GetFiles(x).ToList()).ToList();
        }
    }
}
