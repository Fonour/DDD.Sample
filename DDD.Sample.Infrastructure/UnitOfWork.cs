﻿using DDD.Sample.Domain;
using DDD.Sample.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Sample.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContext _dbContext;
        private DbContextTransaction _dbTransaction;

        public UnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbTransaction = _dbContext.Database.BeginTransaction();
        }

        public async Task<bool> RegisterNew<TEntity>(TEntity entity)
            where TEntity : class
        {
            _dbContext.Set<TEntity>().Add(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RegisterDirty<TEntity>(TEntity entity)
            where TEntity : class
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RegisterClean<TEntity>(TEntity entity)
            where TEntity : class
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Unchanged;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RegisterDeleted<TEntity>(TEntity entity)
            where TEntity : class
        {
            _dbContext.Set<TEntity>().Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void Commit()
        {
            _dbTransaction.Commit();
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
        }
    }
}
