namespace Application.BP.Bases.Caching;

using Common.BP.Bases;
using Service;
using System.Net.Http;

/// <summary>
/// Application layer cache manager service.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IApiCachingAppService<out TResponse> : IAppService
    where TResponse : BaseResponse
{
    /// <summary>
    /// Check cache for value and create and entry if none have been found.
    /// </summary>
    /// <param name="param">parameters of the function</param>
    /// <param name="route">The endpoint route.</param>
    /// <param name="serviceMethod">The method to be called to populate the data. must return a <see cref="TResponse"/> or will throw exception.</param>
    /// <param name="userIdentifier">Anything that can identify the user.</param>
    /// <returns></returns>
    public TResponse Cache(Delegate serviceMethod, string route, string userIdentifier, params object[] param);
}