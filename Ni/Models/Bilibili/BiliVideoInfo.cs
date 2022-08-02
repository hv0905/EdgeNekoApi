using System.Text.Json.Serialization;

namespace Ni.Models.Bilibili;

public class BiliVideoInfo
{
    [JsonPropertyName("comment")]
    public int Comment { get; set; }
    [JsonPropertyName("typeid")]
    public int Typeid { get; set; }
    [JsonPropertyName("play")]
    public int Play { get; set; }
    [JsonPropertyName("pic")]
    public string Pic { get; set; }
    [JsonPropertyName("subtitle")]
    public string Subtitle { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("copyright")]
    public int Copyright { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("review")]
    public int Review { get; set; }
    [JsonPropertyName("author")]
    public string Author { get; set; }
    [JsonPropertyName("mid")]
    public int Mid { get; set; }
    [JsonPropertyName("created")]
    public int Created { get; set; }
    [JsonPropertyName("length")]
    public string Length { get; set; }
    [JsonPropertyName("video_review")]
    public int VideoReview { get; set; }
    [JsonPropertyName("aid")]
    public int Aid { get; set; }
    [JsonPropertyName("bvid")]
    public string Bvid { get; set; }
    [JsonPropertyName("hide_click")]
    public bool HideClick { get; set; }
    [JsonPropertyName("is_pay")]
    public int IsPay { get; set; }
    [JsonPropertyName("is_union_video")]
    public int IsUnionVideo { get; set; }
    [JsonPropertyName("is_steins_gate")]
    public int IsSteinsGate { get; set; }
    [JsonPropertyName("is_live_playback")]
    public int IsLivePlayback { get; set; }
}