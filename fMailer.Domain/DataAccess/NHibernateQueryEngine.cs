namespace fMailer.Domain.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using NHibernate;
    using NHibernate.Linq;
    using fMailer.Domain.Model;
    using System.Diagnostics;

    public class NHibernateQueryEngine : IQueryEngine
    {
        private readonly ISession session;

        public NHibernateQueryEngine(ISession session)
        {
            this.session = session;
            session.Transaction.Begin();
        }

        public TEntity GetById<TEntity>(int id) where TEntity : class, IUnique
        {
            return session.Get<TEntity>(id);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return session.Query<TEntity>();
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            session.Save(entity);
        }
        
        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            session.SaveOrUpdate(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            session.Delete(entity);
        }

        public void BeginTransaction()
        {
            if (!session.Transaction.IsActive)
            {
                session.Transaction.Begin();
            }
            else
            {
                Debug.WriteLine("Transaction is already started!");
            }
        }

        public void CommitTransaction()
        {
            if (session.Transaction.IsActive)
            {
                session.Transaction.Commit();
                session.Transaction.Dispose();
            }
            else
            {
                Debug.WriteLine("There are no started transactions!");
            }
        }

        public void RollBackTransaction()
        {
            session.Transaction.Rollback();
        }

        public void Submit()
        {
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Commit();
                session.Transaction.Dispose();
            }
            else
            {
                Debug.WriteLine("There are no started transactions!");
            }
        }

        public void Dispose()
        {
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Commit();
                session.Transaction.Dispose();
            }

            session.Dispose();
        }
    }
}