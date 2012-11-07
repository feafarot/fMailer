namespace fMailer.Domain.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using fMailer.Domain.Model;

    public interface IQueryEngine : IDisposable
    {
        TEntity GetById<TEntity>(int id) where TEntity : class, IUnique;

        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;

        void Add<TEntity>(TEntity entity) where TEntity : class;

        void Delete<TEntity>(TEntity entity) where TEntity : class;

        void BeginTransaction();

        void CommitTransaction();

        void RollBackTransaction();

        void Submit();
    }
}
