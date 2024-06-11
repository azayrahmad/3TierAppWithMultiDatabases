using Data.Models.ProductDb;
using Data.Models.TransactionDb;
using Data.Models.UserDb;
using Services.Repositories;

namespace Services.UnitOfWork
{
    /// <summary>
    /// Interface Unit of Work to handle all async transactions
    /// </summary>
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Product> Products { get; }
        IRepository<Transaction> Transactions { get; }
        Task CompleteAsync();
    }
}