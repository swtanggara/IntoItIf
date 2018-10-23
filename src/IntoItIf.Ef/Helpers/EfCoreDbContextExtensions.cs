#if NETSTANDARD2_0
namespace IntoItIf.Ef.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain.Entities;
   using Base.Domain.Options;
   using Base.UnitOfWork;
   using DbContexts;
   using Microsoft.EntityFrameworkCore;
   using UnitOfWork;

   internal static class EfCoreDbContextExtensions
   {
      #region Methods

      internal static Option<bool> AddEntry<T>(this Option<ItsDbContext> dataContext, Option<T> entity)
         where T : class
      {
         try
         {
            dataContext.Map(x => x.Set<T>()).Map(x => x.Add(entity.ReduceOrDefault()));
            return true;
         }
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      internal static Option<int> ExecuteSqlCommand(
         this Option<ItsDbContext> dataContext,
         Option<string> sql,
         params Option<object>[] parameters)
      {
         return dataContext
            .Map(
               x =>
               {
                  var inSql = sql.ReduceOrDefault();
                  var inParameters = parameters.ToOptionOfEnumerable().ReduceOrDefault();
                  return x.Database.ExecuteSqlCommand(inSql, inParameters);
               });
      }

      internal static Task<Option<int>> ExecuteSqlCommandAsync(
         this Option<ItsDbContext> dataContext,
         Option<string> sql,
         params Option<object>[] parameters)
      {
         return dataContext.ExecuteSqlCommandAsync(None.Value, sql, parameters);
      }

      internal static Task<Option<int>> ExecuteSqlCommandAsync(
         this Option<ItsDbContext> dataContext,
         Option<CancellationToken> ctok,
         Option<string> sql,
         params Option<object>[] parameters)
      {
         return dataContext
            .MapAsync(
               x =>
               {
                  var inSql = sql.ReduceOrDefault();
                  var inParameters = parameters.ToOptionOfEnumerable().ReduceOrDefault();
                  var inCtok = ctok.Reduce(CancellationToken.None);
                  return x.Database.ExecuteSqlCommandAsync(inSql, inParameters, inCtok);
               });
      }

      internal static Option<List<T>> FromSql<T>(
         this Option<ItsDbContext> dataContext,
         Option<string> sql,
         params Option<object>[] parameters)
         where T : class
      {
         return dataContext
            .Map(
               x =>
               {
                  var inSql = sql.ReduceOrDefault();
                  var inParameters = parameters.ToOptionOfEnumerable().ReduceOrDefault();
                  return x.Set<T>().FromSql(inSql, inParameters).ToList();
               });
      }

      internal static Task<Option<List<T>>> FromSqlAsync<T>(
         this Option<ItsDbContext> dataContext,
         Option<string> sql,
         params Option<object>[] parameters)
         where T : class
      {
         return dataContext.FromSqlAsync<T>(None.Value, sql, parameters);
      }

      internal static Task<Option<List<T>>> FromSqlAsync<T>(
         this Option<ItsDbContext> dataContext,
         Option<CancellationToken> ctok,
         Option<string> sql,
         params Option<object>[] parameters)
         where T : class
      {
         return dataContext
            .MapAsync(
               x =>
               {
                  var inSql = sql.ReduceOrDefault();
                  var inParameters = parameters.ToOptionOfEnumerable().ReduceOrDefault();
                  var inCtok = ctok.Reduce(CancellationToken.None);
                  return x.Set<T>().FromSql(inSql, inParameters).ToListAsync(inCtok);
               });
      }

      internal static Option<IUowDbTransaction> GetUowDbTransaction(this Option<ItsDbContext> dataContext)
      {
         return dataContext
            .Map(
               x =>
               {
                  var trx = x.Database.BeginTransaction();
                  return new UowCoreDbTransaction(trx);
               })
            .ReduceOrDefault();
      }

      internal static Option<bool> RemoveEntry<T>(this Option<ItsDbContext> dataContext, Option<T> exist)
         where T : class
      {
         try
         {
            dataContext.Map(x => x.Set<T>()).Map(x => x.Remove(exist.ReduceOrDefault()));
            return true;
         }
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      internal static Option<int> SaveChanges(this Option<ItsDbContext> dataContext)
      {
         return dataContext.Map(x => x.SaveChanges());
      }

      internal static Task<Option<int>> SaveChangesAsync(this Option<ItsDbContext> dataContext)
      {
         return dataContext.MapAsync(x => x.SaveChangesAsync());
      }

      internal static Task<Option<int>> SaveChangesAsync(
         this Option<ItsDbContext> dataContext,
         Option<CancellationToken> ctok)
      {
         return dataContext.MapAsync(x => x.SaveChangesAsync(ctok.ReduceOrDefault()));
      }

      internal static Option<bool> UpdateEntry<T>(
         this Option<ItsDbContext> dataContext,
         Option<T> entity,
         Option<T> exist)
         where T : class
      {
         try
         {
            dataContext
               .Map(
                  x =>
                  {
                     var inEntity = entity.ReduceOrDefault();
                     var entry = x.Entry(exist.ReduceOrDefault());
                     entry.CurrentValues.SetValues(inEntity);
                     if (!(inEntity is IRowVersion rowVersionEntity)) return true;
                     entry.OriginalValues["RowVersion"] = rowVersionEntity.RowVersion;
                     return true;
                  });
            return true;
         }
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      #endregion
   }
}
#endif