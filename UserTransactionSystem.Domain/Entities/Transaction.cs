using UserTransactionSystem.Domain.Enums;

namespace UserTransactionSystem.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}