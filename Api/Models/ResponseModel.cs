namespace Api.Models;

public record ResponseSuccessModel(int ParsedCount, object ParsedContent)
{
    public StatusType Status { get; init; } = StatusType.Success;
}

public record ResponseFailedModel(string Message)
{
    public StatusType Status { get; init; } = StatusType.Failed;
}