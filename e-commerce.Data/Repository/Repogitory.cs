﻿using e_commerce.Data.Context;
using e_commerce.Domain.Interfaces;
using e_commerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            if(entity == null)
                return false;
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            if (entity == null)
                return false;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(TEntity entity)
        {
            if(entity == null)
                return false;
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Product>> GetProductsInCategoryAsync(int categoryId)
        {
            return await _context.Products.Include(p => p.Category).Where(product => product.CategoryId == categoryId).ToListAsync();
        }

        public async Task<List<Product>> GetProductsSortedByPriceAsync(bool ascending = true)
        {
            var query = _context.Products.AsQueryable();
            if (ascending)
            {
                query = query.OrderBy(product => product.Price);
            }
            else
            {
                query = query.OrderByDescending(product => product.Price);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsSortedByNameAsync(bool ascending = true)
        {
            var query = _context.Products.AsQueryable();
            if (ascending)
            {
                query = query.OrderBy(product => product.Name);
            }
            else
            {
                query = query.OrderByDescending(product => product.Name);
            }

            return await query.ToListAsync();
        }
    }
}
