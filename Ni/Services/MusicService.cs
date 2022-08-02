using System.Net;
using System.Runtime.CompilerServices;
using Ni.Models;
using Ni.Models.Bilibili;
using Ni.Models.Config;

namespace Ni.Services
{
    public class MusicService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MusicService> _logger;

        private const int BILI_TID = 3;

        public List<Music> AvailMusics { get; set; } = new();

        public MusicService(IConfiguration configuration, IHttpClientFactory httpClient, ILogger<MusicService> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClient;
            _logger = logger;
        }

        public async Task UpdateMusic()
        {
            _logger.LogInformation("Now updating music...");
            List<Music> musicList = new();
            var conf = _configuration.GetSection("SongProvider").Get<SongProviderConfig>();
            musicList.AddRange(await LoadBilibiliMusic(conf.Bilibili));
            AvailMusics = musicList;
            _logger.LogInformation($"Update success. Collected {AvailMusics.Count} musics.");
        }

        private async Task<List<Music>> LoadBilibiliMusic(List<string> uids)
        {
            List<Music> musics = new();
            var client = _httpClientFactory.CreateClient();
            
            foreach (var uid in uids)
            {
                _logger.LogInformation("Collect music from uid: " + uid);
                int pn = 1;
                // get music result from uid
                while (true)
                {
                    try
                    {
                        var url =
                            $"https://api.bilibili.com/x/space/arc/search?mid={uid}&ps=50&tid=3&pn={pn}&order=pubdate";
                        var response = await client.GetFromJsonAsync<BiliSearchApiModel>(url);
                        if (response.Code != 0)
                        {
                            throw new Exception($"BiliApi error! \n Trying get uid: {uid} \n Code: {response.Code}\n Message: {response.Message}");
                        }

                        var count = response.Data.Page.Count;
                        foreach (var item in response.Data.List.VList)
                        {
                            var music = new Music
                            {
                                Id = item.Aid,
                                Name = item.Title,
                                AuthorName = item.Author,
                                CoverUrl = item.Pic,
                                Url = $"https://www.bilibili.com/video/{item.Bvid}",
                                Source = "bilibili",
                                Description = item.Description
                            };
                            musics.Add(music);
                        }

                        if (50 * pn >= count)
                        {
                            break;
                        }
                        ++pn;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                    }
                }
            }

            return musics;
        }
    }
}
