using UserTransactionSystem.Domain.Enums;

namespace UserTransactionSystem.Services.DTOs
{
    public class TransactionTypeTotalAmountReportDto
    {
        public TransactionTypeEnum TransactionType { get; set; }
        public decimal TotalTransactionAmount { get; set; }
    }
}