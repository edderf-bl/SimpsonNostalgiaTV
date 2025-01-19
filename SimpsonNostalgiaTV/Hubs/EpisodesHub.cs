using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using SimpsonNostalgiaTV.Models;
using SimpsonNostalgiaTV.Models.Configuration;
using SimpsonNostalgiaTV.Utils;
using System.Text.Json;

namespace SimpsonNostalgiaTV.Hubs
{
    public class EpisodesHub : Hub
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOptionsMonitor<SerieConfiguration> _serieConfig;

        public static Episode? EpisodePlay = null;

        public EpisodesHub(IWebHostEnvironment webHostEnvironment, IOptionsMonitor<SerieConfiguration> serieConfig)
        {
            _webHostEnvironment = webHostEnvironment;
            _serieConfig = serieConfig;
        }

        public override async Task OnConnectedAsync()
        {
        }

        public async Task GetRandomEpisode()
        {
            if (EpisodePlay == null || EpisodePlay.DatePlay.Add(EpisodePlay.Duration) <= DateTime.UtcNow)
                EpisodePlay = EpisodeUtils.GetRandomEpisode(_webHostEnvironment.WebRootPath, _serieConfig.CurrentValue.Name);

            await Clients.All.SendAsync("ReceiveEpisode",$"{JsonSerializer.Serialize(EpisodePlay)}");
        }

    }
}
