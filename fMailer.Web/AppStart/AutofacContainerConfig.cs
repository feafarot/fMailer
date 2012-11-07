// ------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Castle.Components.DictionaryAdapter;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using fMailer.Domain.DataAccess;
using fMailer.Domain.ModelMappings;
using fMailer.Web.Core.Settings;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using fMailer.Web.Core.HashProviders;
using fMailer.Web.Core;

namespace fMailer.Web
{
    public class AutofacContainerConfig
    {
        public static void InitializeContainer()
        {
            var builder = new ContainerBuilder();
            var currentAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterModelBinders(currentAssembly);
            builder.RegisterModelBinderProvider();
            builder.RegisterControllers(currentAssembly);

            Registerdependencies(ref builder);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            DependencyResolver.Current.GetServices<ISessionFactory>();
        }

        private static void Registerdependencies(ref ContainerBuilder builder)
        {
            builder.RegisterType<MD5HashProvider>().As<IHashProvider>().SingleInstance();
            builder.RegisterType<SessionManager>().As<ISessionManager>().InstancePerDependency();
            builder.RegisterType<NHibernateQueryEngine>().As<IQueryEngine>().InstancePerDependency().PropertiesAutowired(PropertyWiringFlags.PreserveSetValues);
            builder.RegisterType<DataRepository>().As<IRepository>().InstancePerDependency().OnRelease(x => x.Dispose());
            builder.RegisterInstance(GetCitadelsConfigurationAdapter()).As<IMailerSettings>().SingleInstance();
            builder.Register(c => CreateSessionFactory()).As<ISessionFactory>().SingleInstance();
            builder.Register(c => c.Resolve<ISessionFactory>().OpenSession()).As<ISession>().InstancePerDependency();
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                           .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("fMailerConnectionString")))
                           .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMapping>().Conventions.Add(ForeignKey.EndsWith("Id")))
                           //.ExposeConfiguration(c => new SchemaExport(c).Create(false, true))
                           .ExposeConfiguration(c => new SchemaUpdate(c).Execute(false, true))
                           .BuildSessionFactory();
        }

        private static IMailerSettings GetCitadelsConfigurationAdapter()
        {
            var citadelsSettings = new DictionaryAdapterFactory().GetAdapter<IMailerSettings>(ConfigurationManager.AppSettings);
            return citadelsSettings;
        }
    }
}