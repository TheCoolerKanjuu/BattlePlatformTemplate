﻿namespace Application.BP.Bases.Caching;

using Common.BP.Bases;
using Domain.BP.Bases.Caching;
using Microsoft.Extensions.Logging;
using Service;

/// <inheritdoc cref="IApiCachingAppService{TResponse}"/>/>
public class ApiCachingAppService<TResponse> : AppService, IApiCachingAppService<TResponse>
    where TResponse : BaseResponse
{
    /// <summary>
    /// Instance of the caching domain service.
    /// </summary>
    private readonly IApiCachingDomainService<TResponse> _cachingDomainService;

    /// <summary>
    /// Instantiate the api caching app service.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="cachingDomainService">The caching domain service.</param>
    public ApiCachingAppService(
        ILogger<ApiCachingAppService<TResponse>> logger,
        IApiCachingDomainService<TResponse> cachingDomainService) 
        : base(logger)
    {
        this._cachingDomainService = cachingDomainService;
    }

    /// <inheritdoc/>
    public TResponse Cache(Func<TResponse> serviceMethod, string route, string userIdentifier = "")
    {
        var key = BuildKey(route, userIdentifier);
        var res = this._cachingDomainService.Get(key);
        if (res != null) return res;
        var dbres = serviceMethod();
        dbres.Source = "Database";
        return dbres;

    }
    
    /// <inheritdoc/>
    public TResponse Cache(Func<object, TResponse> serviceMethod, object param, string route, string userIdentifier = "")
    {
        var key = BuildKey(route, userIdentifier);
        var res = this._cachingDomainService.Get(key);
        if (res != null) return res;
        var dbres = serviceMethod(param);
        dbres.Source = "Database";
        return dbres;
    }
    /// <inheritdoc/>
    public TResponse Cache(Func<int, TResponse> serviceMethod, int param, string route, string userIdentifier = "")
    {
        var key = BuildKey(route, userIdentifier);
        var res = this._cachingDomainService.Get(key);
        if (res != null) return res;
        var dbres = serviceMethod(param);
        dbres.Source = "Database";
        return dbres;
    }

    private static string BuildKey(string route, string userIdentifier)
    {
        return $"route:{route}user:{userIdentifier}";
    }
}