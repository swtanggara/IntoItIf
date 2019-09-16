#if NETSTANDARD2_0
namespace IntoItIf.Ef.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain.Entities;
   using Base.UnitOfWork;
   using DbContexts;
   using Microsoft.EntityFrameworkCore;
   using UnitOfWork;

   public static class EfCoreDbContextExtensions
   {
      #region Methods

      public static bool AddEntry<T>(this IItsDbContext dataContext, T entity)
         where T : class
      {
         try
         {
            dataContext.Set<T>().Add(entity);
            return true;
         }
         catch (Exception)
         {
            return false;
         }
      }

      public static int ExecuteSqlCommand(
         this IItsDbContext dataContext,
         string sql,
         params object[] parameters)
      {
         return dataContext.Database.ExecuteSqlCommand(sql, parameters);
      }

      public static Task<int> ExecuteSqlCommandAsync(
         this IItsDbContext dataContext,
         string sql,
         params object[] parameters)
      {
         return dataContext.ExecuteSqlCommandAsync(CancellationToken.None, sql, parameters);
      }

      public static Task<int> ExecuteSqlCommandAsync(
         this IItsDbContext dataContext,
         CancellationToken ctok,
         string sql,
         params object[] parameters)
      {
         return dataContext.Database.ExecuteSqlCommandAsync(sql, parameters, ctok);
      }

      public static List<T> FromSql<T>(
         this IItsDbContext dataContext,
         string sql,
         params object[] parameters)
         where T : class
      {
         return dataContext.Set<T>().FromSql(sql, parameters).ToList();
      }

      public static Task<List<T>> FromSqlAsync<T>(
         this IItsDbContext dataContext,
         string sql,
         params object[] parameters)
         where T : class
      {
         return dataContext.FromSqlAsync<T>(CancellationToken.None, sql, parameters);
      }

      internal static Task<List<T>> FromSqlAsync<T>(
         this IItsDbContext dataContext,
         CancellationToken ctok,
         string sql,
         params object[] parameters)
         where T : class
      {
         return dataContext.Set<T>().FromSql(sql, parameters).ToListAsync(ctok);
      }

      public static IUowDbTransaction GetUowDbTransaction(this IItsDbContext dataContext)
      {
         var trx = dataContext.Database.BeginTransaction();
         return new UowCoreDbTransaction(trx);
      }

      public static bool RemoveEntry<T>(this IItsDbContext dataContext, T exist)
         where T : class
      {
         try
         {
            dataContext.Set<T>().Remove(exist);
            return true;
         }
         catch (Exception)
         {
            return false;
         }
      }

      public static int SaveChanges(this IItsDbContext dataContext)
      {
         return dataContext.SaveChanges();
      }

      public static Task<int> SaveChangesAsync(this IItsDbContext dataContext)
      {
         return dataContext.SaveChangesAsync(CancellationToken.None);
      }

      public static Task<int> SaveChangesAsync(
         this IItsDbContext dataContext,
         CancellationToken ctok)
      {
         return dataContext.SaveChangesAsync(ctok);
      }

      public static bool UpdateEntry<T>(
         this IItsDbContext dataContext,
         T entity,
         T exist)
         where T : class
      {
         try
         {
            var entry = dataContext.Entry(exist);
            entry.CurrentValues.SetValues(entity);
            if (!(entity is IRowVersion rowVersionEntity)) return true;
            entry.OriginalValues["RowVersion"] = rowVersionEntity.RowVersion;
            return true;
         }
         catch (Exception)
         {
            return false;
         }
      }

      #endregion
   }
}
#endif