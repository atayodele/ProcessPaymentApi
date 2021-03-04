using CheckoutApp.Extensions;
using CheckoutApp.Models;
using CheckoutApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CheckoutApp.Context
{
    public class DataContext : DbContext, IDbContext
    {
        private IDbContextTransaction _transaction;
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<CardPayment> CardPayments { get; set; }
        public DbSet<PaymentState> PaymentStates { get; set; }
        public DbSet<PaymentModelState> PaymentModelStates { get; set; } 
        public DbSet<GatewayMethod> GatewayMethods { get; set; }

        public DataContext()
        {
            Database.EnsureDeleted(); // delete the database with the old schema
            Database.EnsureCreated(); // create a database with a new schema
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            CreateDefaultState(modelBuilder);
            CreateDefaultGatewayMethods(modelBuilder);
        }

        public static void CreateDefaultState(ModelBuilder builder)
        {
            List<PaymentModelState> listState = new List<PaymentModelState>
            {
                new PaymentModelState { Name = "pending" },
                new PaymentModelState { Name = "processed" },
                new PaymentModelState { Name = "failed" },

            };
            List<PaymentModelState> payment = new List<PaymentModelState>();
            foreach (var item in listState)
            {
                var ID = Guid.NewGuid();
                var pay = (PaymentModelState)item;
                pay.Id = ID;
                pay.Name = item.Name;
                payment.Add(pay);
            }
            builder.Entity<PaymentModelState>().HasData(payment.ToArray());
        }
        public static void CreateDefaultGatewayMethods(ModelBuilder builder)
        {
            List<GatewayMethod> gatewayList = new List<GatewayMethod>
            {
                new GatewayMethod { Name = "ICheapPaymentGateway", Status = true },
                new GatewayMethod { Name = "IExpensivePaymentGateway", Status = true },
                new GatewayMethod { Name = "PremiumPaymentGateway", Status = true }
            };
            List<GatewayMethod> gateway = new List<GatewayMethod>();
            foreach (var item in gatewayList)
            {
                var ID = Guid.NewGuid();
                var gate = (GatewayMethod)item;
                gate.Id = ID;
                gate.Name = item.Name;
                gate.Status = item.Status;
                gateway.Add(gate);
            }
            builder.Entity<GatewayMethod>().HasData(gateway.ToArray());
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public void SetAsAdded<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            UpdateEntityState(entity, EntityState.Added);
        }

        public void SetAsModified<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            UpdateEntityState(entity, EntityState.Modified);
        }

        public void SetAsDeleted<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            UpdateEntityState(entity, EntityState.Deleted);
        }


        public void SetAsDetached<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            UpdateEntityState(entity, EntityState.Detached);
        }

        public void BeginTransaction()
        {
            _transaction = this.Database.BeginTransaction();
        }

        public int Commit()
        {
            var saveChanges = SaveChanges();
            _transaction.Commit();
            return saveChanges;
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public Task<int> CommitAsync()
        {
            var saveChangesAsync = SaveChangesAsync();
            _transaction.Commit();
            return saveChangesAsync;
        }

        private void UpdateEntityState<TEntity>(TEntity entity, EntityState entityState) where TEntity : BaseEntity
        {
            //this.ChangeTracker.TrackGraph(entity, e => ApplyStateUsingIsKeySet(e.Entry));
            var dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = entityState;
        }

        private EntityEntry GetDbEntityEntrySafely<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var dbEntityEntry = Entry<TEntity>(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                Set<TEntity>().Attach(entity);
            }
            return dbEntityEntry;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
