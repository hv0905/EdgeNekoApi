namespace Ni.Services;

public class MusicUpdateService : IHostedService
{
    private readonly MusicService _musicService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MusicUpdateService> _logger;
    private Timer? _timer;

    public MusicUpdateService(MusicService musicService, IConfiguration configuration, ILogger<MusicUpdateService> logger)
    {
        _musicService = musicService;
        _configuration = configuration;
        _logger = logger;
    }

    private async void Tick()
    {
        await _musicService.UpdateMusic();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // get interval
        await _musicService.UpdateMusic();
        var interval = TimeSpan.FromSeconds(_configuration.GetValue("MusicUpdateInterval", 7200));
        _timer = new(_ => Tick(), null, interval,interval);
        _logger.LogInformation($"Music Update Interval has set to {interval.TotalSeconds} seconds.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Music Update Service stopped.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
}