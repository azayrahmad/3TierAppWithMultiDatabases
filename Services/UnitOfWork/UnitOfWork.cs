using Data.Contexts;
using Data.Models.ProductDb;
using Data.Models.TransactionDb;
using Data.Models.UserDb;
using Services.Repositories;
using Services.Repositories.Interfaces;
using System.Transactions;

namespace Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserDbContext _userDbContext;
        private readonly ProductDbContext _productDbContext;
        private readonly TransactionDbContext _transactionDbContext;

        public UnitOfWork(UserDbContext userDbContext, ProductDbContext productDbContext, TransactionDbContext transactionDbContext)
        {
            _userDbContext = userDbContext;
            _productDbContext = productDbContext;
            _transactionDbContext = transactionDbContext;
            Users = new UserRepository(_userDbContext);
            Products = new ProductRepository(_productDbContext);
            Categories = new CategoryRepository(_productDbContext);
            Transactions = new TransactionRepository(_transactionDbContext);
        }

        public IUserRepository Users  { get; private set; }
        public IProductRepository Products  { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public ITransactionRepository Transactions  { get; private set; }

        public async Task CompleteAsync()
        {
            using TransactionScope transactionScope = new();
            await _userDbContext.SaveChangesAsync();
            await _productDbContext.SaveChangesAsync();
            await _transactionDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _userDbContext.Dispose();
            _productDbContext.Dispose();
            _transactionDbContext.Dispose();
        }
    }
}
