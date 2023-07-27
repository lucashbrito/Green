using Green.Domain.Abstractions;

namespace Green.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GreenDbContext _context;

        public UnitOfWork(GreenDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
