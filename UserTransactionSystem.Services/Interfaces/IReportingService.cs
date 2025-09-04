using UserTransactionSystem.Services.DTOs;

namespace UserTransactionSystem.Services.Interfaces
{
    public interface IReportingService
    {
        Task<IEnumerable<UserTotalAmountReportDto>> GetTotalTransactionsAmountByUserAsync();
        Task<IEnumerable<TransactionTypeTotalAmountReportDto>> GetTotalTransactionsAmountByTypeAsync();
        Task<HighVolumeTransactionReportDto> GetHighVolumeTransactionsAsync(DateTime from, DateTime to, decimal threshHoldAmount);
    }
}