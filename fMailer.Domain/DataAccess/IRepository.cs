namespace fMailer.Domain.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using fMailer.Domain.Model;

    public interface IRepository : IDisposable
    {
        TEntity GetById<TEntity>(int id) where TEntity : class, IUnique;

        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;

        void Add<TEntity>(TEntity entity) where TEntity : class;

        void Delete<TEntity>(TEntity entity) where TEntity : class;

        void DeleteAll<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        void Transaction(Action transactionAction);

        void Submit();
    }
}