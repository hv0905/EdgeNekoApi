namespace Ni.Models;

public class Music
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? AuthorName { get; set; }
    public string? Url { get; set; }
    public string? CoverUrl { get; set; }
    public string? Source { get; set; }
    public string? Description { get; set; }
    public DateTime PublishTime { get; set; }
    public string? EmbedPlayerUrl { get; set; }

}
