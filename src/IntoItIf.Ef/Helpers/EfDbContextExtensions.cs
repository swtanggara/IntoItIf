#if NET471
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
   using UnitOfWork;

   internal static class EfDbContextExtensions
   {
      #region Methods

      internal static bool AddEntry<T>(this ItsDbContext dataContext, T entity)
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

      internal static int ExecuteSqlCommand(
         this ItsDbContext dataContext,
         string sql,
         params object[] parameters)
      {
         return dataContext.Database.ExecuteSqlCommand(sql, parameters);
      }

      internal static Task<int> ExecuteSqlCommandAsync(
         this ItsDbContext dataContext,
         string sql,
         params object[] parameters)
      {
         return dataContext.ExecuteSqlCommandAsync(CancellationToken.None, sql, parameters);
      }

      internal static Task<int> ExecuteSqlCommandAsync(
         this ItsDbContext dataContext,
         CancellationToken ctok,
         string sql,
         params object[] parameters)
      {
         return dataContext.Database.ExecuteSqlCommandAsync(sql, ctok, parameters);
      }

      internal static List<T> FromSql<T>(
         this ItsDbContext dataContext,
         string sql,
         params object[] parameters)
         where T : class
      {
         return dataContext.Set<T>().SqlQuery(sql, parameters).ToList();
      }

      internal static Task<List<T>> FromSqlAsync<T>(
         this ItsDbContext dataContext,
         string sql,
         params object[] parameters)
         where T : class
      {
         return dataContext.FromSqlAsync<T>(CancellationToken.None, sql, parameters);
      }

      internal static Task<List<T>> FromSqlAsync<T>(
         this ItsDbContext dataContext,
         CancellationToken ctok,
         string sql,
         params object[] parameters)
         where T : class
      {
         return dataContext.Set<T>().SqlQuery(sql, parameters).ToListAsync(ctok);
      }

      internal static IUowDbTransaction GetUowDbTransaction(this ItsDbContext dataContext)
      {
         var trx = dataContext.Database.BeginTransaction();
         return new UowDbTransaction(trx);
      }

      internal static bool RemoveEntry<T>(this ItsDbContext dataContext, T exist)
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

      internal static int SaveChanges(this ItsDbContext dataContext)
      {
         return dataContext.SaveChanges();
      }

      internal static Task<int> SaveChangesAsync(this ItsDbContext dataContext)
      {
         return dataContext.SaveChangesAsync();
      }

      internal static Task<int> SaveChangesAsync(
         this ItsDbContext dataContext,
         CancellationToken ctok)
      {
         return dataContext.SaveChangesAsync(ctok);
      }

      internal static bool UpdateEntry<T>(
         this ItsDbContext dataContext,
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