namespace UserTransactionSystem.Services.DTOs
{
    public class UserTotalAmountReportDto
    {
        public Guid UserId { get; set; }
        public decimal TotalTransactionAmount { get; set; }
    }
}