namespace Domain.BP.Bases.Caching;

using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Common.BP.Bases;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service;
using StackExchange.Redis;

/// <inheritdoc cref="IApiCachingDomainService{TResponse}"/>/>
public class ApiCachingDomainService<TResponse> : DomainService, IApiCachingDomainService<TResponse>
    where TResponse : BaseResponse
{
    /// <summary>
    /// Reference to the caching database.
    /// </summary>
    private readonly IDatabase _cachingDb;

    /// <summary>
    /// Instantiate the api caching domain service.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="cacheService">redis access service singleton.</param>
    public ApiCachingDomainService(
        ILogger<ApiCachingDomainService<TResponse>> logger, 
        IConnectionMultiplexer cacheService)
        : base(logger)
    {
        this._cachingDb = cacheService.GetDatabase();
    }
    
    /// <inheritdoc/>
    public TResponse? Get(string key)
    {
        var cache = this._cachingDb.StringGet(key);
        if (string.IsNullOrEmpty(cache)) return null;
        var res = JsonConvert.DeserializeObject<TResponse>(cache);
        if (res == null) throw new SerializationException();
        res.Source = "Cache";
        return res;
    }
    
    /// <inheritdoc/>
    public void Store(string key, TResponse res, int expirationTime = -1)
    {
        if( expirationTime > 0)
            this._cachingDb.StringSet(key, JsonConvert.SerializeObject(res), expiry: TimeSpan.FromSeconds(expirationTime));
        else
            this._cachingDb.StringSet(key, JsonConvert.SerializeObject(res));
    }

    /// <inheritdoc/>
    public async Task<TResponse?> GetAsync(string key)
    {
        var cache = await this._cachingDb.StringGetAsync(key);
        if (string.IsNullOrEmpty(cache)) return null;
        var res = JsonConvert.DeserializeObject<TResponse>(cache);
        if (res == null) throw new SerializationException();
        res.Source = "Cache";
        return res;
    }
    
    /// <inheritdoc/>
    public async Task StoreAsync(string key, TResponse res, int expirationTime = -1)
    {
        if( expirationTime > 0)
            await this._cachingDb.StringSetAsync(key, JsonConvert.SerializeObject(res), expiry: TimeSpan.FromSeconds(expirationTime));
        else
            await this._cachingDb.StringSetAsync(key, JsonConvert.SerializeObject(res));
    }
}