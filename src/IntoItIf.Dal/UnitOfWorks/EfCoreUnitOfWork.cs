#if NETSTANDARD2_0

// ReSharper disable SuspiciousTypeConversion.Global
namespace IntoItIf.Dal.UnitOfWorks
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Options;
   using DbContexts;
   using Exceptions;
   using Microsoft.EntityFrameworkCore;
   using Microsoft.EntityFrameworkCore.Storage;
   using Microsoft.EntityFrameworkCore.Update;

   public sealed class EfCoreUnitOfWork : BaseUnitOfWork
   {
      #region Constructors and Destructors

      public EfCoreUnitOfWork(Option<IDbContext> dbContext) : base(dbContext)
      {
      }

      #endregion

      #region Public Methods and Operators

      public override Option<int> ExecuteSqlCommand(Option<string> sql, params Option<object>[] parameters)
      {
         return sql.Combine(parameters.ToOptionOfEnumerable(), true)
            .Combine(GetDbContext<EfCoreDbContext>())
            .Map(x => (Sql: x.Item1.Item1, Parameters: x.Item1.Item2, DbContext: x.Item2))
            .Map(
               x => x.DbContext.Database.ExecuteSqlCommand(x.Sql, x.Parameters));
      }

      public override Task<Option<int>> ExecuteSqlCommandAsync(
         Option<CancellationToken> ctok,
         Option<string> sql,
         params Option<object>[] parameters)
      {
         return sql.Combine(parameters.ToOptionOfEnumerable(), true)
            .Combine(ctok, true, CancellationToken.None)
            .Combine(GetDbContext<EfCoreDbContext>())
            .Map(
               x => (
                  Sql: x.Item1.Item1.Item1,
                  Parameters: x.Item1.Item1.Item2,
                  DbContext: x.Item2,
                  Ctok: x.Item1.Item2
               ))
            .MapAsync(
               x => x.DbContext.Database.ExecuteSqlCommandAsync(x.Sql, x.Parameters, x.Ctok));
      }

      public override Option<List<T>> FromSql<T>(Option<string> sql, params Option<object>[] parameters)
      {
         return sql.Combine(parameters.ToOptionOfEnumerable(), true)
            .Map(x => (Sql: x.Item1, Parameters: x.Item2))
            .Map(
               x => GetDbContext<EfCoreDbContext>().ReduceOrDefault().Set<T>().FromSql(x.Sql, x.Parameters).ToList());
      }

      public override Task<Option<List<T>>> FromSqlAsync<T>(Option<string> sql, params Option<object>[] parameters)
      {
         return FromSqlAsync<T>(None.Value, sql, parameters);
      }

      public override Task<Option<List<T>>> FromSqlAsync<T>(
         Option<CancellationToken> ctok,
         Option<string> sql,
         params Option<object>[] parameters)
      {
         return ctok.Combine(sql)
            .Combine(parameters.ToOptionOfEnumerable(), true)
            .Map(x => (Sql: x.Item1.Item2, Parameters: x.Item2, Ctok: x.Item1.Item1))
            .MapAsync(
               x => GetDbContext<EfCoreDbContext>()
                  .ReduceOrDefault()
                  .Set<T>()
                  .FromSql(x.Sql, x.Parameters)
                  .ToListAsync(x.Ctok));
      }

      public override Option<int> SaveChanges()
      {
         try
         {
            return GetDbContext<EfCoreDbContext>().Map(x => x.SaveChanges());
         }
         catch (DbUpdateConcurrencyException ex)
         {
            return Fail<int>.Throw(
               new EfCoreDbUpdateConcurrencyException(ex.Message, ex.Entries.Select(x => (IUpdateEntry)x).ToList()));
         }
         catch (DbUpdateException ex)
         {
            return Fail<int>.Throw(
               new EfCoreDbUpdateException(ex.Message, ex, ex.Entries.Select(x => (IUpdateEntry)x).ToList()));
         }
         catch (Exception ex)
         {
            return Fail<int>.Throw(ex);
         }
      }

      public override Task<Option<int>> SaveChangesAsync()
      {
         return SaveChangesAsync(ctok: None.Value);
      }

      public override async Task<Option<int>> SaveChangesAsync(Option<CancellationToken> ctok)
      {
         try
         {
            return await GetDbContext<EfCoreDbContext>()
               .Combine(ctok)
               .MapAsync(x => x.Item1.SaveChangesAsync(x.Item2));
         }
         catch (DbUpdateConcurrencyException ex)
         {
            return Fail<int>.Throw(
               new EfCoreDbUpdateConcurrencyException(ex.Message, ex.Entries.Select(x => (IUpdateEntry)x).ToList()));
         }
         catch (DbUpdateException ex)
         {
            return Fail<int>.Throw(
               new EfCoreDbUpdateException(ex.Message, ex, ex.Entries.Select(x => (IUpdateEntry)x).ToList()));
         }
         catch (Exception ex)
         {
            return Fail<int>.Throw(ex);
         }
      }

      public override Task<Option<int>> SaveChangesAsync(params Option<BaseUnitOfWork>[] unitOfWorks)
      {
         return SaveChangesAsync(None.Value, unitOfWorks);
      }

      public override Task<Option<int>> SaveChangesAsync(
         Option<CancellationToken> ctok,
         params Option<BaseUnitOfWork>[] unitOfWorks)
      {
         return ctok.Combine(unitOfWorks.ToOptionOfEnumerable(), true)
            .Combine(GetDbContext<EfCoreDbContext>())
            .Map(
               x => (DbContext: x.Item2, UowS: x.Item1.Item2, Ctok: x.Item1.Item1))
            .MapAsync(
               async x =>
               {
                  // TransactionScope will be included in .NET Core v2.0
                  using (var transaction = x.DbContext.Database.BeginTransaction())
                  {
                     try
                     {
                        var count = 0;
                        foreach (var unitOfWork in x.UowS)
                        {
                           if (!(unitOfWork is EfCoreUnitOfWork uow)) continue;
                           uow.GetDbContext<EfCoreDbContext>()
                              .ReduceOrDefault()
                              .Database.UseTransaction(transaction.GetDbTransaction());
                           count += (await uow.SaveChangesAsync(x.Ctok).ConfigureAwait(false)).ReduceOrDefault();
                        }

                        count += (await SaveChangesAsync(x.Ctok).ConfigureAwait(false)).ReduceOrDefault();
                        transaction.Commit();
                        return count;
                     }
                     catch (DbUpdateConcurrencyException ex)
                     {
                        transaction.Rollback();
                        throw new EfCoreDbUpdateConcurrencyException(
                           ex.Message,
                           ex.Entries.Select(y => (IUpdateEntry)y).ToList());
                     }
                     catch (DbUpdateException ex)
                     {
                        transaction.Rollback();
                        throw new EfCoreDbUpdateException(
                           ex.Message,
                           ex,
                           ex.Entries.Select(y => (IUpdateEntry)y).ToList());
                     }
                     catch (Exception ex)
                     {
                        transaction.Rollback();
                        // ReSharper disable once PossibleIntendedRethrow
                        throw ex;
                     }
                  }
               });
      }

      #endregion
   }
}
#endif