namespace Data.Models.TransactionDb
{
    /// <summary>
    /// Store each transaction of a product by a user.
    /// </summary>
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public required string Status { get; set; }
    }
}
