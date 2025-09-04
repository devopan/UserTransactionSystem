using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using UserTransactionSystem.Services.DTOs;
using UserTransactionSystem.Services.Interfaces;

namespace UserTransactionSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportingController : ControllerBase
    {
        private readonly IReportingService _reportingService;
        private const string UserString = "user";
        private const string TransactionTypeString = "transactiontype";

        public ReportingController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        /// <summary>
        /// Calculate the total transaction amount for each user.
        /// </summary>
        [HttpGet("users/transactionAmountTotal")]
        public async Task<ActionResult<IEnumerable<UserTotalAmountReportDto>>> GetTotalTransactionsAmountByUser()
        {
            var report = await _reportingService.GetTotalTransactionsAmountByUserAsync();
            return Ok(report);
        }

        /// <summary>
        /// Calculate the total transaction amount for each transaction type.
        /// </summary>
        [HttpGet("transactionTypes/transactionAmountTotal")]
        public async Task<ActionResult<IEnumerable<TransactionTypeTotalAmountReportDto>>> GetTotalTransactionsAmountByType()
        {
            var report = await _reportingService.GetTotalTransactionsAmountByTypeAsync();
            return Ok(report);
        }

        /// <summary>
        /// Identify transactions above a certain threshold amount.
        /// </summary>
        /// <param name="from">Enter a 'from' date using the format dd/MM/yyyy.</param>
        /// <param name="to">Enter a 'to' date using the format dd/MM/yyyy.</param>
        /// <param name="thresholdAmount">Enter a Decimal 'threshold amount'.</param>
        [HttpGet("high-volume-transactions")]
        public async Task<ActionResult<HighVolumeTransactionReportDto>> GetHighVolumeTransactions(
            [FromQuery] string from,
            [FromQuery] string to,
            [FromQuery] decimal thresholdAmount)
        {
            try
            {
                // Parse dates from dd/MM/yyyy format
                if (!DateTime.TryParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
                {
                    return BadRequest("Invalid 'from' date format. Use dd/MM/yyyy.");
                }

                if (!DateTime.TryParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
                {
                    return BadRequest("Invalid 'to' date format. Use dd/MM/yyyy.");
                }

                // Set time to start of day for 'from' and end of day for 'to'
                fromDate = fromDate.Date;
                toDate = toDate.Date.AddDays(1).AddTicks(-1); // End of the day

                var report = await _reportingService.GetHighVolumeTransactionsAsync(fromDate, toDate, thresholdAmount);
                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
    }
}
