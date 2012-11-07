namespace fMailer.Domain.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class DataRepository : IRepository
    {
        private readonly IQueryEngine queryEngine;

        private bool isInTransaction;

        public DataRepository(IQueryEngine queryEngine)
        {
            this.queryEngine = queryEngine;
        }

        public virtual TEntity GetById<TEntity>(int id) where TEntity : class, IUnique
        {
            return queryEngine.GetById<TEntity>(id);
        }
        
        public virtual IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return queryEngine.GetAll<TEntity>();
        }

        public virtual void Add<TEntity>(TEntity entity) where TEntity : class
        {
            BeginTransaction();
            queryEngine.Add(entity);
            CommitTransaction();
        }

        public virtual void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            BeginTransaction();
            queryEngine.Delete(entity);
            CommitTransaction();
        }

        public virtual void DeleteAll<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities == null)
            {
                return;
            }

            var list = entities.ToList();

            BeginTransaction();
            foreach (var entity in list)
            {
                queryEngine.Delete(entity);
            }

            CommitTransaction();
        }

        public void Transaction(Action transactionAction)
        {
            isInTransaction = true;
            queryEngine.BeginTransaction();
            try
            {
                transactionAction();
                queryEngine.CommitTransaction();
            }
            catch (Exception)
            {
                queryEngine.RollBackTransaction();
                throw;
            }
            finally
            {
                isInTransaction = false;
            }
        }

        public virtual void Submit()
        {
            ////queryEngine.Submit();
        }

        public virtual void Dispose()
        {
            queryEngine.Dispose();
        }

        private void BeginTransaction()
        {
            if (!isInTransaction)
            {
                queryEngine.BeginTransaction();
            }
        }

        private void CommitTransaction()
        {
            if (!isInTransaction)
            {
                queryEngine.CommitTransaction();
            }
        }
    }
}