using UserTransactionSystem.Domain.Entities;
using AutoMapper;
using UserTransactionSystem.Infrastructure.UnitOfWork;
using UserTransactionSystem.Services.DTOs;
using UserTransactionSystem.Services.Interfaces;

namespace UserTransactionSystem.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _unitOfWork.Transactions.GetAllAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            
            if (transaction?.Id != null)
            {
                return transaction;
            }

            return null;
        }

        public async Task<Transaction> CreateTransactionAsync(CreateTransactionDto createTransactionDto)
        {
            var transaction = _mapper.Map<Transaction>(createTransactionDto);
            var existingUsers = await _unitOfWork.Users.FindAsync(x => x.Id == createTransactionDto.UserId);
            if (existingUsers.Count() == 0)
            {
                return null;
            }
            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.CompleteAsync();
            return transaction;
        }
    }
}