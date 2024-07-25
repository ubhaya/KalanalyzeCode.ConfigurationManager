namespace KalanalyzeCode.ConfigurationManager.Entity.Abstract;

public interface IResponseModel
{
    public bool Success { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

public interface IResponseDataModel<T> : IResponseModel where T : class
{
    public T? Data { get; set; }
}