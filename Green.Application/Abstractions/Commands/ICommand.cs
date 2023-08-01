using Green.Domain.Shared;
using MediatR;

namespace Green.Application.Abstractions.Commands;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

