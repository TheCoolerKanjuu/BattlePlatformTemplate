using Environment = Common.BF.Enums.Environment;

namespace Presentation.BF.Helpers;

public static class EnvFinder
{
    public static Environment GetEnvironment(IWebHostEnvironment builderEnvironment)
    {
        if (builderEnvironment.IsProduction())
            return Environment.Production;
        
        if (builderEnvironment.IsStaging())
            return Environment.Staging;
        
        if (builderEnvironment.IsEnvironment("UnitTest"))
            return Environment.Test;
        
        if (builderEnvironment.IsEnvironment("PreProduction"))
            return Environment.PreProduction;
        
        if (builderEnvironment.IsEnvironment("Validation"))
            return Environment.Validation;
        
        if (builderEnvironment.IsEnvironment("Integration"))
            return Environment.Integration;

        return Environment.Development;
    }
}