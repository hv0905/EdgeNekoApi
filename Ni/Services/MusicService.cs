using Ni.Models;
using Ni.Models.Bilibili;
using Ni.Models.Config;

namespace Ni.Services;

public class MusicService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MusicService> _logger;

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
        if (conf?.Bilibili != null)
        {
            musicList.AddRange(await LoadBilibiliMusic(conf.Bilibili));
        }
        AvailMusics = musicList;
        _logger.LogInformation($"Update success. Collected {AvailMusics.Count} musics.");
    }

    private async Task<IEnumerable<Music>> LoadBilibiliMusic(List<string> uids)
    {
        List<Music> musics = new();
        var client = _httpClientFactory.CreateClient();

        foreach (var uid in uids)
        {
            _logger.LogInformation("Collect music from uid: " + uid);
            var pn = 1;
            // get music result from uid
            try
            {
                while (true)
                {
                    var url =
                        $"https://api.bilibili.com/x/space/arc/search?mid={uid}&ps=50&tid=3&pn={pn}&order=pubdate";
                    var response = await client.GetFromJsonAsync<BiliSearchApiModel>(url);
                    if (response!.Code != 0)
                    {
                        throw new Exception(
                            $"BiliApi error! \n Trying get uid: {uid} \n Code: {response.Code}\n Message: {response.Message}");
                    }

                    musics.AddRange(response.Data.List.VList.Select(t => new Music()
                    {
                        Id = t.Aid,
                        Name = t.Title,
                        AuthorName = t.Author,
                        CoverUrl = t.Pic,
                        Url = $"https://www.bilibili.com/video/{t.Bvid}",
                        Source = "bilibili",
                        Description = t.Description
                    }));

                    if (50 * pn >= response.Data.Page.Count) // last page
                    {
                        break;
                    }

                    ++pn;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
        
        return musics.DistinctBy(t => t.Url);;
    }
}