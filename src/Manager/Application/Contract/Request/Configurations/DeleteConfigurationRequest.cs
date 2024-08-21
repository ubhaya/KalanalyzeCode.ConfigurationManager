using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;

public sealed record DeleteConfigurationRequest(Guid Id)
    : IRequest;