﻿using CheckoutApp.Models;
using CheckoutApp.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CheckoutApp.Services.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContext _context;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
        }

        private bool _disposed;
        private object syncRoot = new object();

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            return new EntityRepository<TEntity>(_context);
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public void BeginTransaction()
        {
            _context.BeginTransaction();
        }

        public int Commit()
        {
            return _context.Commit();
        }

        public void Rollback()
        {
            _context.Rollback();
        }

        public Task<int> CommitAsync()
        {
            return _context.CommitAsync();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        {
            GC.SuppressFinalize(this);
        }
    }
}
