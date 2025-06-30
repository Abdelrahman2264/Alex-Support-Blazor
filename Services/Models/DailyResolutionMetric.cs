namespace AlexSupport.Services.Models
{
    public class DailyResolutionMetric
    {
        public DateTime Date { get; set; }
        public int SolvedTickets { get; set; }
        public int TotalTickets { get; set; }
    }
}
