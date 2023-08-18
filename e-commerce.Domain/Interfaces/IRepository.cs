using e_commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// it return list of entity class
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// it retrun entity based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(int id);
        /// <summary>
        /// it add entity to database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddAsync(TEntity entity);
        /// <summary>
        /// it update entity in database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity);
        /// <summary>
        /// It delete entity from database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(TEntity entity);
        /// <summary>
        /// it return all category of product
        /// </summary>
        /// <returns></returns>
        Task<List<Category>> GetAllCategory();
        /// <summary>
        /// it return IQueriable of Product from database
        /// </summary>
        /// <returns></returns>
        IQueryable<Product> GetAllProduct();
    }
}
