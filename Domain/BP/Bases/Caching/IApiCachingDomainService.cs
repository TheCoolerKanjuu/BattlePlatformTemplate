namespace Domain.BP.Bases.Caching;

using Service;

/// <summary>
/// Generic service handling api based caching.
/// </summary>
public interface IApiCachingDomainService<TResponse> : IDomainService
{
    /// <summary>
    /// Asynchronously get entry from cache and returns it as a nullable <see cref="TResponse"/>.
    /// </summary>
    /// <param name="key">The access key of the value.</param>
    /// <returns></returns>
    public Task<TResponse?> GetAsync(string key);
    
    
    /// <summary>
    /// Get entry from cache and returns it as a nullable <see cref="TResponse"/>.
    /// </summary>
    /// <param name="key">The access key of the value.</param>
    /// <returns></returns>
    public TResponse? Get(string key);

    /// <summary>
    /// Asynchronously store a <see cref="TResponse"/> at the specified key location.
    /// </summary>
    /// <param name="key">The access key of the value.</param>
    /// <param name="res">The response to be stored.</param>
    /// <param name="expirationTime">expiration time of the cache in seconds, if not provided or a non positive value is, the cache will not expire.</param>
    /// <returns></returns>
    public Task StoreAsync(string key, TResponse res,int expirationTime = -1);

    /// <summary>
    /// Store a <see cref="TResponse"/> at the specified key location.
    /// </summary>
    /// <param name="key">The access key of the value.</param>
    /// <param name="res">The response to be stored.</param>
    /// <param name="expirationTime">expiration time of the cache in seconds, if not provided or a non positive value is, the cache will not expire.</param>
    /// <returns></returns>
    public void Store(string key, TResponse res, int expirationTime = -1);
}