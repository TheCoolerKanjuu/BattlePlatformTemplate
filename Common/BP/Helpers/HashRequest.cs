namespace Common.BP.Helpers;

using static System.String;

public static class HashRequest
{
    public static string BuildAnonymousKey(string routeType, string route, string requestBody ="")
    {
        return HashCode.Combine(route, routeType, requestBody).ToString();
    }
}