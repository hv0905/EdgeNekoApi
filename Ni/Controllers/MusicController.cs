﻿using System.ComponentModel.DataAnnotations;
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
    [Route("/music/RandomSong/{author?}")]
    public NiProtocol RandomSong(string? author)
    {
        var collection = _musicService.AvailMusics;
        
        if (!string.IsNullOrEmpty(author))
        {
            collection = collection.Where(m => m.AuthorName?.Contains(author) ?? false).ToList();
        }
        var index = Random.Shared.Next(0, collection.Count);
        var music = collection[index];
        return new NiDataProtocol<Music>
        {
            Data = music,
            Code = ErrorCode.Success,
            Message = "Successfully get a random song, Enjoy!"
        };
    }
    
    [HttpGet]
    [Route("/music/{id}")]
    [Route("/music/ById/{id}")]
    public NiProtocol ById([Required]int id)
    {
        if (id < 0 || id >= _musicService.AvailMusics.Count)
        {
            return new NiDataProtocol<Music>
            {
                Code = ErrorCode.InvalidInput,
                Message = "Invalid parameter"
            };
        }
        var music = _musicService.AvailMusics[id];
        return new NiDataProtocol<Music>
        {
            Data = music,
            Code = ErrorCode.Success,
            Message = "Successfully get a song, Enjoy!"
        };
    }

    [HttpGet]
    [Route("/music/ByName/{criteria}/{count?}")]
    public NiProtocol ByName([Required] string criteria, int count = 10)
    {
        if (string.IsNullOrEmpty(criteria))
        {
            return new NiDataProtocol<Music>
            {
                Code = ErrorCode.InvalidInput,
                Message = "Invalid parameter"
            };
        }

        var music = _musicService.AvailMusics
            .Where(t => (t.Name?.Contains(criteria) ?? false) || (t.Description?.Contains(criteria) ?? false))
            .OrderByDescending(t => t.PublishTime)
            .Take(count)
            .ToList();
        if (music.Count == 0)
        {
            return new NiProtocol()
            {
                Code = ErrorCode.NotFound,
                Message = "No song found with the given criteria"
            };
        }
        return new NiDataProtocol<List<Music>>
        {
            Code = ErrorCode.Success,
            Message = $"Successfully find {music.Count} songs with the given criteria",
            Data = music
        };
    }
    
    [HttpGet]
    [Route("/music/ByAuthor/{author}/{count?}")]
    [Route("/music/ByAuthor/")]
    public NiProtocol ByAuthor([Required]string author, int count = 3)
    {
        if (string.IsNullOrEmpty(author))
        {
            return new NiDataProtocol<Music>
            {
                Code = ErrorCode.InvalidInput,
                Message = "Invalid parameter"
            };
        }

        var music = _musicService.AvailMusics
            .Where(t => (t.AuthorName?.Contains(author) ?? false) || (t.Description?.Contains(author) ?? false)) // for co-author
            .OrderByDescending(t => t.PublishTime)
            .Take(count)
            .ToList();
        if (music.Count == 0)
        {
            return new NiProtocol()
            {
                Code = ErrorCode.NotFound,
                Message = "No song found with the given criteria"
            };
        }
        return new NiDataProtocol<List<Music>>
        {
            Code = ErrorCode.Success,
            Message = $"Successfully find {music.Count} songs with the given criteria",
            Data = music
        };
    }
    
    [HttpGet]
    [Route("/music/Latest/{count?}")]
    public NiProtocol Latest(int count = 3)
    {
        var music = _musicService.AvailMusics
            .OrderByDescending(t => t.PublishTime)
            .Take(count)
            .ToList();
        return new NiDataProtocol<List<Music>>
        {
            Code = ErrorCode.Success,
            Message = $"Successfully find {music.Count} newest song.",
            Data = music
        };
    }
}