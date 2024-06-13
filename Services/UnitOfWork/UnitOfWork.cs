using Data.Contexts;
using Data.Models.ProductDb;
using Data.Models.TransactionDb;
using Data.Models.UserDb;
using Services.Repositories;

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
            Users = new GenericRepository<User, UserDbContext>(_userDbContext);
            Products = new GenericRepository<Product, ProductDbContext>(_productDbContext);
            Transactions = new GenericRepository<Transaction, TransactionDbContext>(_transactionDbContext);
        }

        public IRepository<User> Users  { get; private set; }
        public IRepository<Product> Products  { get; private set; }
        public IRepository<Transaction> Transactions  { get; private set; }

        public async Task CompleteAsync()
        {
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
