using Application.BP.Bases.Crud;
using Application.BP.Bases.Service;
using Domain.BP.Bases.Crud;
using Domain.BP.Bases.Service;
using Infrastructure.BP.Bases.GenericRepository;
using Infrastructure.BP.Bases.UnitOfWork;
using Environment = Common.BP.Enums.Environment;

namespace Presentation.BP;

using Domain.BP.Bases.Caching;
using StackExchange.Redis;

public static class Ioc
{
    public static void AddDependencyGroup(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        AddApplicationServices(services, environment);
        AddDomainServices(services, environment);
        AddInfraServices(services, environment);
        AddPresentationServices(services, environment, configuration.GetSection("Redis:ConnexionURL").Value!);
    }

    private static void AddInfraServices(IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

    }

    private static void AddPresentationServices(IServiceCollection services, IWebHostEnvironment environment, string redisConnectionUrl)
    {
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionUrl));
    }

    private static void AddDomainServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddScoped<IDomainService, DomainService>();
        services.AddScoped(typeof(ICrudDomainService<,,>), typeof(CrudDomainService<,,>));
        services.AddScoped(typeof(IApiCachingDomainService<>), typeof(ApiCachingDomainService<>));

    }

    private static void AddApplicationServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddScoped<IAppService, AppService>();
        services.AddScoped(typeof(ICrudAppService<,,>), typeof(CrudAppService<,,>));
    }
}