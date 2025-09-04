using UserTransactionSystem.Infrastructure.Repositories;
using UserTransactionSystem.Domain.Entities;

namespace UserTransactionSystem.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Transaction> Transactions { get; }
        Task<int> CompleteAsync();
    }
}