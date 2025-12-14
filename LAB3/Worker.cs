using lab3_2.api.Services;

namespace lab3_2;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();
            var tcpServer = scope.ServiceProvider.GetRequiredService<TcpServer>();
            await tcpServer.StartAsync(stoppingToken);
        }
    }
}
