using UserTransactionSystem.Domain.Entities;
using UserTransactionSystem.Services.DTOs;
using UserTransactionSystem.Services.Interfaces;

namespace UserTransactionSystem.Web.Controllers
{
    public class TransactionsController : BaseController<Transaction, CreateTransactionDto, int>
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService actionService)
        {
            _transactionService = actionService;
        }

        protected override async Task<Transaction> ReadSingleAsync(int id)
        {
            return await _transactionService.GetTransactionByIdAsync(id);
        }

        protected override async Task<Transaction> CreateAsync(CreateTransactionDto createDto)
        {
            return await _transactionService.CreateTransactionAsync(createDto);
        }

        protected override int GetEntityId(Transaction entity)
        {
            return entity.Id;
        }

        protected override async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _transactionService.GetAllTransactionsAsync();
        }
    }
}