namespace Common.BP.Response;

using Bases;

public record DtoListResponse<TDto> : BaseResponse
    where TDto : BaseDto
{
    public required IEnumerable<TDto> Data { get; set; }
}