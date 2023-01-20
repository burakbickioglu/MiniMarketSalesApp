namespace BaseProject.BO.Models;

public class CommandBase<T>
{
}
public interface IGenericApiResult
{
    public bool IsSucceed { get; set; }
    public string Message { get; set; }
    public IGenericApiResult SetFailed(string message);
    public IGenericApiResult SetMessage(string message);
}

public class GenericApiResult<T> : GenericApiResult where T : class
{
    public T? Data { get; set; }

    public static GenericApiResult<T> GetSucceed(string message, T data)
    {
        return new GenericApiResult<T>(true, message, data);
    }
    public static GenericApiResult<T> GetSucceed(T data)
    {
        return new GenericApiResult<T>(true, string.Empty, data);
    }
    public static GenericApiResult<T> GetFailed(string? message, T? data = null)
    {
        return new GenericApiResult<T>(false, message ?? string.Empty, data);
    }
    public static new GenericApiResult<T> GetFailed(Exception? e) => GetFailed(JsonConvert.SerializeObject(e));
    public static new GenericApiResult<T> NotFound() => GetFailed("", null);

    public GenericApiResult() : base()
    {
    }
    private GenericApiResult(bool isSucceed, string message, T? data) : base(isSucceed, message)
    {
        Data = data;
    }
}

public class GenericApiResult : IGenericApiResult
{
    public bool IsSucceed { get; set; }
    public string Message { get; set; }
    public static GenericApiResult GetSucceed(string message)
    {
        return new GenericApiResult(true, message);
    }
    public static GenericApiResult GetSucceed()
    {
        return new GenericApiResult(true, string.Empty);
    }
    public IGenericApiResult SetFailed(string message)
    {
        IsSucceed = false;
        Message = message;
        return this;
    }
    public IGenericApiResult SetMessage(string message)
    {
        Message = message;
        return this;
    }

    public static GenericApiResult GetFailed(string message)
    {
        return new GenericApiResult(false, message);
    }
    public static GenericApiResult GetFailed(Exception x)
    {
        return new GenericApiResult(false, x.Message);
    }
    public static GenericApiResult NotFound()
    {
        return new GenericApiResult(false, "");
    }
    public GenericApiResult()
    {
        IsSucceed = false;
        Message = string.Empty;
    }
    protected GenericApiResult(bool isSucceed, string message)
    {
        IsSucceed = isSucceed;
        Message = message;
    }

}
