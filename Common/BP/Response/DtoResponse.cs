namespace Common.BP.Response;

using Bases;

public record DtoResponse<TDto> : BaseResponse
    where TDto : BaseDto
{
    public required TDto Data { get; set; }
}