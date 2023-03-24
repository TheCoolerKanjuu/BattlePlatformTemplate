using Environment = Common.BF.Enums.Environment;

namespace Presentation;

public static class Ioc
{
    public static void AddDependencyGroup(this IServiceCollection services, Environment environment)
    {
        BF.Ioc.AddDependencyGroup(services, environment);
        AddDomainServices(services, environment);
        AddApplicationServices(services, environment);
    }

    private static void AddDomainServices(this IServiceCollection services, Environment environment)
    {
        
    }
    
    private static void AddApplicationServices(this IServiceCollection services, Environment environment)
    {
        
    }
}