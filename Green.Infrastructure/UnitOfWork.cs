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

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
