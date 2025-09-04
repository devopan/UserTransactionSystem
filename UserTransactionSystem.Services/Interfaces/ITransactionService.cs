using UserTransactionSystem.Domain.Entities;
using UserTransactionSystem.Services.DTOs;

namespace UserTransactionSystem.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task<Transaction> CreateTransactionAsync(CreateTransactionDto createActionDto);
    }
}
