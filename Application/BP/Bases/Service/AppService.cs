using Microsoft.Extensions.Logging;

namespace Application.BP.Bases.Service;

public class AppService : IAppService
{
    protected readonly ILogger<AppService> Logger;

    public AppService(ILogger<AppService> logger)
    {
        this.Logger = logger;
    }

}