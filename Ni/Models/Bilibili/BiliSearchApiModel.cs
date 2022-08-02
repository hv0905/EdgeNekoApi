namespace Ni.Models.Bilibili;

public class BiliSearchApiModel
{
    public int Code { get; set; }
    public string Message { get; set; }
    public int Ttl { get; set; }
    public BiliSearchData Data { get; set; }
}