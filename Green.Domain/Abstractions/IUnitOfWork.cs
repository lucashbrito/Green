namespace Green.Domain.Abstractions;

public interface IUnitOfWork
{
    Task CompleteAsync(CancellationToken cancellationToken);
}
