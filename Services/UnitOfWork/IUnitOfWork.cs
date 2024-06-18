using Services.Repositories.Interfaces;

namespace Services.UnitOfWork
{
    /// <summary>
    /// Interface Unit of Work to handle all async transactions
    /// </summary>
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        ITransactionRepository Transactions { get; }
        ICategoryRepository Categories { get; }
        Task CompleteAsync();
        void Dispose();
    }
}