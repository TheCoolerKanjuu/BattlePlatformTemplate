using Application.BP.Bases.Crud;
using Application.BP.Bases.Service;
using Domain.BP.Bases.Crud;
using Domain.BP.Bases.Service;
using Infrastructure.BP.Bases.GenericRepository;
using Infrastructure.BP.Bases.UnitOfWork;
using Environment = Common.BP.Enums.Environment;

namespace Presentation.BP;

public static class Ioc
{
    public static void AddDependencyGroup(this IServiceCollection services, Environment environment)
    {
        AddApplicationServices(services, environment);
        AddDomainServices(services, environment);
        AddInfraServices(services, environment);
    }

    private static void AddInfraServices(IServiceCollection services, Environment environment)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

    }

    private static void AddDomainServices(this IServiceCollection services, Environment environment)
    {
        services.AddScoped<IDomainService, DomainService>();
        services.AddScoped(typeof(ICrudDomainService<,,>), typeof(ICrudDomainService<,,>));

    }
    
    private static void AddApplicationServices(this IServiceCollection services, Environment environment)
    {
        services.AddScoped<IAppService, AppService>();
        services.AddScoped(typeof(ICrudAppService<,,>), typeof(ICrudAppService<,,>));
    }
}