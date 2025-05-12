namespace Domain.Responses;

public class ResponseResult
{
    public bool Success { get; set; }

    public int? StatusCode { get; set; }

    public string? Error { get; set; }
}

public class ResponseResult<T> : ResponseResult
{
    public T? Result { get; set; }
}
