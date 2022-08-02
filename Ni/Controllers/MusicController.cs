using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ni.Models;
using Ni.Services;

namespace Ni.Controllers;

public class MusicController : ControllerBase
{
    private readonly MusicService _musicService;

    public MusicController(MusicService musicService)
    {
        _musicService = musicService;
    }

    [HttpGet]
    public async Task<NiProtocol> RandomSong()
    {
        if (_musicService.AvailMusics.Count == 0)
        {
            await _musicService.UpdateMusic();
        }
        
        var random = new Random();
        var index = random.Next(0, _musicService.AvailMusics.Count);
        var music = _musicService.AvailMusics[index];
        music.Id = index;
        return new NiDataProtocol<Music>()
        {
            Data = music,
            Code = ErrorCode.Success,
            Message = "Successfully get a random song, Enjoy!"
        };
    }
}
