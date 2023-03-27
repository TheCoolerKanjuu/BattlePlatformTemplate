namespace Common.BP.Request;

using Bases;

public class LazyLoadRequest : BaseRequest
{
    public required int Page { get; set; }
    
    public required int PageSize { get; set; }

    public string Filter { get; set; } = "";

    public string OrderBy { get; set; } = "";
}