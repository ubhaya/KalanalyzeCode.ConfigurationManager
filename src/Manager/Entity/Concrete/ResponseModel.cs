using KalanalyzeCode.ConfigurationManager.Entity.Abstract;

namespace KalanalyzeCode.ConfigurationManager.Entity.Concrete;

public class ResponseModel : IResponseModel
{
    public bool Success { get; set; }
    public IEnumerable<string>? Errors { get; set; }
    
    public static ResponseDataModel<TData> Create<TData>(TData? data) where TData : class
    {
        return new ResponseDataModel<TData>()
        {
            Data = data,
            Success = data is not null
        };
    }  
}

public class ResponseDataModel<T> : ResponseModel, IResponseDataModel<T> where T : class
{
    public T? Data { get; set; }
}