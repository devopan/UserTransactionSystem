using UserTransactionSystem.Domain.Entities;

namespace UserTransactionSystem.Services.DTOs
{
    public class HighVolumeTransactionReportDto
    {
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}