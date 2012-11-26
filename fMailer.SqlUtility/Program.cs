// ------------------------------------------------------------------------
// <copyright file="Program.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Diagnostics;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using fMailer.Domain.ModelMappings;
using NHibernate.Tool.hbm2ddl;

namespace fMailer.SqlUtility
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine(" Database recreating...");
            CreateAndWaitForProc("sqlcmd", "-S .\\ -i RecreateDatabase.sql");
            System.Console.WriteLine(" Database recreated.");

            System.Console.WriteLine(" Creating fMailer schema... ");
            var sessionFactory = Fluently.Configure()
                                         .Database(MsSqlConfiguration.MsSql2008.ShowSql().ConnectionString(c => c.FromConnectionStringWithKey("fMailerConnectionString")))
                                         .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMapping>().Conventions.Add(ForeignKey.EndsWith("Id")))
                                         .ExposeConfiguration(c => new SchemaUpdate(c).Execute(false, true))
                                         .BuildSessionFactory();
            sessionFactory.OpenSession();
            System.Console.WriteLine(" Schema created successfully. ");

            System.Console.WriteLine(" Filling DB with default values...");
            CreateAndWaitForProc("sqlcmd", "-S .\\ -i Defaults.sql");            
            System.Console.WriteLine(" All operations completed successfully.");
            System.Console.WriteLine();
        }

        private static void CreateAndWaitForProc(string cmd, string args)
        {
            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo(cmd, args);
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();
        }
    }
}
