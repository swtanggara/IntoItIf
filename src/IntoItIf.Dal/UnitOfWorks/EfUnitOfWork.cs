// ReSharper disable SuspiciousTypeConversion.Global

#if NET471
namespace IntoItIf.Dal.UnitOfWorks
{
   using System;
   using System.Collections.Generic;
   using System.Data.Entity;
   using System.Data.Entity.Infrastructure;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Options;
   using DbContexts;
   using Exceptions;
   using IntoItIf.Dal.UnitOfWorks;

   public class EfUnitOfWork : BaseUnitOfWork
   {
      #region Constructors and Destructors

      public EfUnitOfWork(Option<IDbContext> dbContext) : base(dbContext)
      {
      }

      #endregion

      #region Public Methods and Operators

      public override Option<int> ExecuteSqlCommand(Option<string> sql, params Option<object>[] parameters)
      {
         return sql.Combine(parameters.ToOptionOfEnumerable(), true)
            .Combine(GetDbContext<EfDbContext>())
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
            .Combine(GetDbContext<EfDbContext>())
            .Map(
               x => (
                  Sql: x.Item1.Item1.Item1,
                  Parameters: x.Item1.Item1.Item2,
                  DbContext: x.Item2,
                  Ctok: x.Item1.Item2
               ))
            .MapAsync(
               x => x.DbContext.Database.ExecuteSqlCommandAsync(x.Sql, x.Ctok, x.Parameters));
      }

      public override Option<List<T>> FromSql<T>(Option<string> sql, params Option<object>[] parameters)
      {
         return sql.Combine(parameters.ToOptionOfEnumerable(), true)
            .Map(x => (Sql: x.Item1, Parameters: x.Item2))
            .Map(
               x => GetDbContext<EfDbContext>().ReduceOrDefault().Set<T>().SqlQuery(x.Sql, x.Parameters).ToList());
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
               x => GetDbContext<EfDbContext>()
                  .ReduceOrDefault()
                  .Set<T>()
                  .SqlQuery(x.Sql, x.Parameters)
                  .ToListAsync(x.Ctok));
      }

      public override IUowDbTransaction GetDbTransaction<TContext>()
      {
         if (GetDbContext<TContext>() is DbContext dbContext)
         {
            var trx = dbContext.Database.BeginTransaction();
            return new UowDbTransaction(trx);
         }

         throw new InvalidOperationException("Invalid DbContext when try to get IUowDbTransaction");
      }

      public override Option<int> SaveChanges()
      {
         try
         {
            return GetDbContext<EfDbContext>().Map(x => x.SaveChanges());
         }
         catch (DbUpdateConcurrencyException ex)
         {
            return Fail<int>.Throw(new EfDbUpdateConcurrencyException(ex.Message, ex));
         }
         catch (DbUpdateException ex)
         {
            return Fail<int>.Throw(new EfDbUpdateException(ex.Message, ex));
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
            return await GetDbContext<EfDbContext>()
               .Combine(ctok)
               .MapAsync(x => x.Item1.SaveChangesAsync(x.Item2));
         }
         catch (DbUpdateConcurrencyException ex)
         {
            return Fail<int>.Throw(new EfDbUpdateConcurrencyException(ex.Message, ex));
         }
         catch (DbUpdateException ex)
         {
            return Fail<int>.Throw(new EfDbUpdateException(ex.Message, ex));
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
            .Combine(GetDbContext<EfDbContext>())
            .Map(
               x => (DbContext: x.Item2, UowS: x.Item1.Item2, Ctok: x.Item1.Item1))
            .MapAsync(
               async x =>
               {
                  using (var transaction = x.DbContext.Database.BeginTransaction())
                  {
                     try
                     {
                        var count = 0;
                        foreach (var unitOfWork in x.UowS)
                        {
                           if (!(unitOfWork is EfUnitOfWork uow)) continue;
                           uow.GetDbContext<EfDbContext>()
                              .ReduceOrDefault()
                              .Database.UseTransaction(transaction.UnderlyingTransaction);
                           count += (await uow.SaveChangesAsync(ctok).ConfigureAwait(false)).ReduceOrDefault();
                        }

                        count += (await SaveChangesAsync(ctok).ConfigureAwait(false)).ReduceOrDefault();
                        transaction.Commit();
                        return count;
                     }
                     catch (DbUpdateConcurrencyException ex)
                     {
                        transaction.Rollback();
                        throw new EfDbUpdateConcurrencyException(ex.Message, ex);
                     }
                     catch (DbUpdateException ex)
                     {
                        transaction.Rollback();
                        throw new EfDbUpdateException(ex.Message, ex);
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