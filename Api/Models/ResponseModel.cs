namespace Api.Models;

public class ResponseModel(StatusType status, int parsedNumber, object parsedData)
{
    public StatusType Status { get; set; } = status;
    public int ParsedNumber { get; set; } = parsedNumber;
    public object ParsedData { get; set; } = parsedData;
}