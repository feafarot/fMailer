// ------------------------------------------------------------------------
// <copyright file="Program.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System;
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
            Console.WriteLine("-Database recreating");
            CreateAndWaitForProc("sqlcmd", "-S .\\SQLEXPRESS -i RecreateDatabase.sql");
            Console.WriteLine("-Database recreated");
            Console.WriteLine();

            System.Console.WriteLine("-Creating fMailer schema");
            var sessionFactory = Fluently.Configure()
                                         .Database(MsSqlConfiguration.MsSql2008.ShowSql().ConnectionString(c => c.FromConnectionStringWithKey("fMailerConnectionString")))
                                         .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMapping>().Conventions.Add(ForeignKey.EndsWith("Id")))
                                         .ExposeConfiguration(c => new SchemaUpdate(c).Execute(false, true))
                                         .BuildSessionFactory();            
            sessionFactory.OpenSession();
            Console.WriteLine("-Schema created successfully.");
            Console.WriteLine();         
            Console.WriteLine("-Filling DB with default values...");
            CreateAndWaitForProc("sqlcmd", "-S .\\SQLEXPRESS -i Defaults.sql");            
            Console.WriteLine("-All operations completed.");
            Console.WriteLine();
        }

        private static void CreateAndWaitForProc(string cmd, string args)
        {            
            var info = new ProcessStartInfo(cmd, args);
            info.CreateNoWindow = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.UseShellExecute = false;
            info.ErrorDialog = false;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            using (var process = Process.Start(info))
            {
                process.BeginOutputReadLine();
                process.OutputDataReceived += (o, e) => 
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        Console.WriteLine(" {0}", e.Data);
                    }
                };
                process.ErrorDataReceived += (o, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        Console.WriteLine(" {0}", e.Data);
                    }
                };
                process.WaitForExit();
            }
        }
    }
}
