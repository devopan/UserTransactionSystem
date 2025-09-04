using UserTransactionSystem.Infrastructure.Data;
using UserTransactionSystem.Infrastructure.Repositories;
using UserTransactionSystem.Domain.Entities;

namespace UserTransactionSystem.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<User> _userRepository;
        private IRepository<Transaction> _actionRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<User> Users => _userRepository ??= new Repository<User>(_context);

        public IRepository<Transaction> Transactions => _actionRepository ??= new Repository<Transaction>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}