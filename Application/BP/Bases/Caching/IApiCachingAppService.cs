namespace Application.BP.Bases.Caching;

using Common.BP.Bases;
using Service;
using System.Net.Http;

/// <summary>
/// Application layer cache manager service.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IApiCachingAppService<TResponse> : IAppService
    where TResponse : BaseResponse
{
    /// <summary>
    /// Check cache for value and create and entry if none have been found.
    /// </summary>
    /// <param name="route">The endpoint route.</param>
    /// <param name="serviceMethod">The method to be called to populate the data.</param>
    /// <param name="userIdentifier">Anything that can identify the user.</param>
    /// <returns></returns>
    public TResponse Cache(Func<TResponse> serviceMethod, string route, string userIdentifier = "");

    /// <summary>
    /// Check cache for value and create and entry if none have been found.
    /// </summary>
    /// <param name="param">parameter of the function</param>
    /// <param name="route">The endpoint route.</param>
    /// <param name="serviceMethod">The method to be called to populate the data.</param>
    /// <param name="userIdentifier">Anything that can identify the user.</param>
    /// <returns></returns>
    public TResponse Cache(Func<object, TResponse> serviceMethod,object param, string route, string userIdentifier = "");
    
    /// <summary>
    /// Check cache for value and create and entry if none have been found.
    /// </summary>
    /// <param name="param">parameter of the function</param>
    /// <param name="route">The endpoint route.</param>
    /// <param name="serviceMethod">The method to be called to populate the data.</param>
    /// <param name="userIdentifier">Anything that can identify the user.</param>
    /// <returns></returns>
    public TResponse Cache(Func<int, TResponse> serviceMethod,int param, string route, string userIdentifier = "");
}