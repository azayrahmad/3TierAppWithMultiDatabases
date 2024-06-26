﻿using Data.Contexts;
using Data.Models.TransactionDb;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces;

namespace Services.Repositories
{
    public class TransactionRepository(TransactionDbContext context) : 
        Repository<Transaction, TransactionDbContext>(context), ITransactionRepository
    {
        private readonly DbSet<Transaction> _dbSet = context.Set<Transaction>();
        public async Task<IEnumerable<Transaction>> GetAllByProductIdAsync(int productId)
        {
            return await _dbSet.Where(t => t.ProductId == productId).ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllByUserIdAsync(int userId)
        {
            return await _dbSet.Where(t => t.UserId == userId).ToListAsync();
        }
    }
}
