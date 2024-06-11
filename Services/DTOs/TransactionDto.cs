namespace Services.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }
}