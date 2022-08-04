using System.Text.Json.Serialization;

namespace Ni.Models.Bilibili;

public class BiliSearchData
{
    public class BiliDataListInfo
    {
        [JsonPropertyName("vlist")]
        public List<BiliVideoInfo>? VList { get; set; }
    }

    public class PageInfo
    {
        [JsonPropertyName("pn")]
        public int Pn { get; set; }
        [JsonPropertyName("ps")]
        public int Ps { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
    [JsonPropertyName("list")]
    public BiliDataListInfo? List { get; set; }
    [JsonPropertyName("page")]
    public PageInfo? Page { get; set; }
}