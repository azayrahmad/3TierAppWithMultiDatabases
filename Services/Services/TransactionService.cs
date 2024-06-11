using Data.Models.TransactionDb;
using Services.UnitOfWork;

namespace Services
{
    public class TransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _unitOfWork.Transactions.GetAllAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _unitOfWork.Transactions.GetByIdAsync(id);
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(transaction.UserId);
            var product = await _unitOfWork.Products.GetByIdAsync(transaction.ProductId);

            if (user != null && product != null)
            {
                await _unitOfWork.Transactions.AddAsync(transaction);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new Exception("User or Product does not exist.");
            }
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            var existingTransaction = await _unitOfWork.Transactions.GetByIdAsync(transaction.Id);
            if (existingTransaction != null)
            {
                existingTransaction.UserId = transaction.UserId;
                existingTransaction.ProductId = transaction.ProductId;
                existingTransaction.Status = transaction.Status;
                existingTransaction.TransactionDate = transaction.TransactionDate;

                await _unitOfWork.Transactions.UpdateAsync(existingTransaction);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new Exception("Transaction does not exist");
            }
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _unitOfWork.Transactions.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
