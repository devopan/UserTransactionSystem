using UserTransactionSystem.Infrastructure.Data;
using UserTransactionSystem.Services.DTOs;
using UserTransactionSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using UserTransactionSystem.Domain.Enums;

namespace UserTransactionSystem.Services.Services
{
    public class ReportingService : IReportingService
    {
        private readonly ApplicationDbContext _context;

        public ReportingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserTotalAmountReportDto>> GetTotalTransactionsAmountByUserAsync()
        {
            var report = await _context.Transactions.AsNoTrackingWithIdentityResolution()
                .GroupBy(ua => ua.UserId)
                .Select(g => new UserTotalAmountReportDto
                {
                    UserId = g.Key,
                    TotalTransactionAmount = g.Sum(x => x.TransactionType == TransactionTypeEnum.Debit ? -x.Amount : x.Amount)
                })
                .ToListAsync();

            return report;
        }

        public async Task<IEnumerable<TransactionTypeTotalAmountReportDto>> GetTotalTransactionsAmountByTypeAsync()
        {
            var report = await _context.Transactions.AsNoTrackingWithIdentityResolution()
                .GroupBy(a => a.TransactionType)
                .Select(g => new TransactionTypeTotalAmountReportDto
                {
                    TransactionType = g.Key,
                    TotalTransactionAmount = g.Sum(x => x.TransactionType == TransactionTypeEnum.Debit ? -x.Amount : x.Amount)
                })
                .ToListAsync();

            return report;
        }

        public async Task<HighVolumeTransactionReportDto> GetHighVolumeTransactionsAsync(DateTime from, DateTime to, decimal threshHoldAmount)
        {
            // Ensure dates are in UTC for consistent comparison
            from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            to = DateTime.SpecifyKind(to, DateTimeKind.Utc);

            // Filter actions by date range and threshold amount
            var transactionsInRange = _context.Transactions.AsNoTrackingWithIdentityResolution()
                .Where(a => a.CreatedAt >= from && a.CreatedAt <= to && a.Amount > threshHoldAmount);

            var result = new HighVolumeTransactionReportDto();

            foreach (var userTransaction in transactionsInRange)
            {
                result.Transactions.Add(userTransaction);
            }

            return result;
        }
    }
}
