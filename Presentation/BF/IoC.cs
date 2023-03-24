using Application.BF.Bases.Crud;
using Application.BF.Bases.Service;
using Domain.BF.Bases.Crud;
using Domain.BF.Bases.Service;
using Infrastructure.BF.Bases.GenericRepository;
using Infrastructure.BF.Bases.UnitOfWork;
using Environment = Common.BF.Enums.Environment;

namespace Presentation.BF;

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