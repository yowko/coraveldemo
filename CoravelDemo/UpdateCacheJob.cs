using Coravel.Invocable;

namespace CoravelDemo;

public class UpdateCacheJob:IInvocable
{
    private readonly ILogger<UpdateCacheJob> _logger;
    private readonly UpdateCacheService _updateCacheService;
    public UpdateCacheJob(ILogger<UpdateCacheJob> logger,UpdateCacheService updateCacheService)
    {
        _logger = logger;
        _updateCacheService = updateCacheService;
    }

    public Task Invoke()
    {
        var cancellationToken = new CancellationToken();
        return _updateCacheService.GetAsync(cancellationToken);
    }
}