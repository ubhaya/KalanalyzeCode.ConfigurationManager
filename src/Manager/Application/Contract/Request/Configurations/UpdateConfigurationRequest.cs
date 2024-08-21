using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;

public sealed record UpdateConfigurationRequest(Guid Id, string Name, string Value)
    : IRequest;