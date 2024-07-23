using MediatR;
using Microsoft.AspNetCore.Http;

namespace KalanalyzeCode.ConfigurationManager.Shared.Contract.Request;

public interface IHttpRequest : IRequest<IResult>
{
    
}