using Environment = Common.BP.Enums.Environment;

namespace Presentation;

public static class Ioc
{
    public static void AddDependencyGroup(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        BP.Ioc.AddDependencyGroup(services, environment, configuration);
        AddDomainServices(services, environment);
        AddApplicationServices(services, environment);
    }

    private static void AddDomainServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        
    }
    
    private static void AddApplicationServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        
    }
}