namespace Api.Models;

public class ResponseSuccessModel(int parsedNumber, object parsedContent)
{
    public StatusType Status { get; set; } = StatusType.Success;
    public int ParsedNumber { get; set; } = parsedNumber;
    public object ParsedContent { get; set; } = parsedContent;
}

public class ResponseFailedModel(string message)
{
    public StatusType Status { get; set; } = StatusType.Failed;
    public string Message { get; set; } = message;
}