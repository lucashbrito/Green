using Green.Domain.Shared;
using MediatR;

namespace Green.Application.Abstractions.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
