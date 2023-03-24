using Microsoft.Extensions.Logging;

namespace Domain.BP.Bases.Service;

public class DomainService : IDomainService
{
    protected readonly ILogger<DomainService> Logger;

    public DomainService(ILogger<DomainService> logger)
    {
        this.Logger = logger;
    }

}