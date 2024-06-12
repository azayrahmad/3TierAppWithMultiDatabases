using Services.DTOs;
using Services.UnitOfWork;

namespace Services.Services
{
    public class TransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _unitOfWork.Transactions.GetAllAsync();
            var users = await _unitOfWork.Users.GetAllAsync();
            var products = await _unitOfWork.Products.GetAllAsync();

            return transactions.Select(t =>
            {
                var user = users.FirstOrDefault(u => u.Id == t.UserId);
                var product = products.FirstOrDefault(p => p.Id == t.ProductId);
                return t.ToDto(user, product);
            });
        }

        public async Task<TransactionDto> GetTransactionByIdAsync(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null) return null;

            var user = await _unitOfWork.Users.GetByIdAsync(transaction.UserId);
            var product = await _unitOfWork.Products.GetByIdAsync(transaction.ProductId);

            return transaction.ToDto(user, product);
        }

        public async Task AddTransactionAsync(TransactionDto transactionDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(transactionDto.UserId);
            var product = await _unitOfWork.Products.GetByIdAsync(transactionDto.ProductId);

            if (user == null || product == null)
            {
                throw new Exception("User or Product does not exist.");
            }

            var transaction = transactionDto.ToEntity();

            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateTransactionAsync(TransactionDto transactionDto)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(transactionDto.Id);
            if (transaction == null)
            {
                throw new Exception("Transaction does not exist.");
            }

            transaction.UserId = transactionDto.UserId;
            transaction.ProductId = transactionDto.ProductId;
            transaction.TransactionDate = transactionDto.TransactionDate;
            transaction.Status = transactionDto.Status;

            await _unitOfWork.Transactions.UpdateAsync(transaction);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _unitOfWork.Transactions.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
