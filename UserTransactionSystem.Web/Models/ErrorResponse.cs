namespace UserTransactionSystem.Web.Models
{
    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }

        public ErrorResponse(Exception exception, int statusCode)
        {
            Type = exception.GetType().Name;
            Title = exception.Message;
            Status = statusCode;
            TraceId = Guid.NewGuid().ToString();
        }

        public ErrorResponse(string title, int statusCode)
        {
            Title = title;
            Status = statusCode;
            TraceId = Guid.NewGuid().ToString();
        }
    }
}