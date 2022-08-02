namespace Ni.Models;

public class HelloApiResult : NiProtocol
{
    public DateTime ServerTime { get; set; }
    public DateTime ServerTimeUtc { get; set; }
    public string? WikiPath { get; set; }

    public string? Mode { get; set; }

    public string? ServerRuntime { get; set; }

}
