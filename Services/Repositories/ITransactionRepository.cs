using Data.Models.TransactionDb;

namespace Services.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetAllByUserIdAsync(int userId);
        Task<IEnumerable<Transaction>> GetAllByProductIdAsync(int productId);
    }
}
