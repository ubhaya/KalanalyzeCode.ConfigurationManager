using KalanalyzeCode.ConfigurationManager.Entity.Abstract;

namespace KalanalyzeCode.ConfigurationManager.Entity.Concrete;

public class ResponseModel : IResponseModel
{
    public bool Success { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

public class ResponseDataModel<T> : ResponseModel, IResponseDataModel<T> where T : class
{
    public T? Data { get; set; }
}