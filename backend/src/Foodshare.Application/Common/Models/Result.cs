namespace Foodshare.Application.Common.Models;

public class Result<T>
{
    private Result(bool succeeded, T? data, IEnumerable<string>? errors)
    {
        Succeeded = succeeded;
        Data = data;
        Errors = errors?.ToArray() ?? [];
    }

    public bool Succeeded { get; init; }
    public string[] Errors { get; init; }
    public T? Data { get; init; }

    public static Result<T> Success(T data) 
        => new Result<T>(true, data, null);
    
    public static Result<T> Failure(IEnumerable<string> errors) 
        => new Result<T>(false, default, errors);
}