using UserTransactionSystem.Domain.Enums;

namespace UserTransactionSystem.Services.DTOs
{
    public class CreateTransactionDto
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
    }
}