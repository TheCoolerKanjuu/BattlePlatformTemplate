namespace Presentation.BP.Helpers;

public static class HashRequest
{
    public static string BuildAnonymousKey(string routeType, string route, string requestBody)
    {
        return $"type:{routeType}:route:{route}:body{requestBody}";
    }
    
    public static string BuildAnonymousKey(string routeType, string route)
    {
        return $"type:{routeType}:route:{route}";
    }
}