using Green.Domain.Shared;
using MediatR;

namespace Green.Application.Abstractions.Queries;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
