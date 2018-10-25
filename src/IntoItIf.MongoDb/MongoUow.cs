namespace IntoItIf.MongoDb
{
   using System;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.DataContexts;
   using Base.Domain.Options;
   using Base.UnitOfWork;

   public sealed class MongoUow : Uow<MongoDataContext>, ITransactionUow
   {
      #region Constructors and Destructors

      public MongoUow(Option<MongoDataContext> dataContext) : base(dataContext)
      {
      }

      #endregion

      #region Public Methods and Operators

      public IUowDbTransaction GetDbTransaction<TContext>()
         where TContext : class, IDataContext
      {
         return DataContext
            .Map(x => new MongoUowTransaction(x.GetExplicitMongoSession().ReduceOrDefault()))
            .ReduceOrDefault();
      }

      public Option<TResult> SaveChangesForScoped<TResult>(Func<MongoUowScoped, Option<TResult>> func)
      {
         return func.ToOption()
            .MapFlatten(
               x =>
               {
                  using (var session = GetDbTransaction<MongoDataContext>())
                  {
                     var mongoSession = (MongoUowTransaction)session;
                     try
                     {
                        var scoped = new MongoUowScoped(this, mongoSession.Transaction.ToOption());
                        var result = x(scoped);
                        mongoSession.Commit();
                        return result;
                     }
                     catch (Exception)
                     {
                        mongoSession.Rollback();
                        throw;
                     }
                  }
               });
      }

      public Task<Option<TResult>> SaveChangesForScopedAsync<TResult>(Func<MongoUowScoped, Task<Option<TResult>>> func)
      {
         return SaveChangesForScopedAsync(func, None.Value);
      }

      public Task<Option<TResult>> SaveChangesForScopedAsync<TResult>(
         Func<MongoUowScoped, Task<Option<TResult>>> func,
         Option<CancellationToken> ctok)
      {
         return func.ToOption()
            .Combine(ctok, true, CancellationToken.None)
            .Map(x => (Func: x.Item1, Ctok: x.Item2))
            .MapFlattenAsync(
               async x =>
               {
                  using (var session = GetDbTransaction<MongoDataContext>())
                  {
                     var mongoSession = (MongoUowTransaction)session;
                     try
                     {
                        var scoped = new MongoUowScoped(this, mongoSession.Transaction.ToOption());
                        var result = await x.Func(scoped);
                        await mongoSession.CommitAsync(x.Ctok);
                        return result;
                     }
                     catch (Exception)
                     {
                        await mongoSession.RollbackAsync(x.Ctok);
                        throw;
                     }
                  }
               });
      }

      public Option<MongoRepository<T>> SetOf<T>()
         where T : class
      {
         return GetRepository<MongoRepository<T>, T>();
      }

      public override Option<int> SaveChanges()
      {
         return DataContext.Map(
            x =>
            {
               if (x.ExplicitSession.IsSome()) return 0;
               if (!x.ImplicitSession.IsSome()) return 0;
               var _ = x.ImplicitSession
                  .IfMap(
                     y => y.IsInTransaction,
                     y =>
                     {
                        try
                        {
                           y.CommitTransaction();
                           return 1;
                        }
                        catch (Exception)
                        {
                           y.AbortTransaction();
                           return 0;
                        }
                     });
               return _.Output.IsSome() ? ((Some<int>)_.Output).Content : 0;
            });
      }

      public Option<int> DiscardChanges()
      {
         return DataContext.Map(
            x =>
            {
               if (x.ExplicitSession.IsSome()) return 0;
               if (!x.ImplicitSession.IsSome()) return 0;
               var _ = x.ImplicitSession
                  .IfMap(
                     y => y.IsInTransaction,
                     y =>
                     {
                        try
                        {
                           y.AbortTransaction();
                           return 1;
                        }
                        catch (Exception)
                        {
                           return 0;
                        }
                     });
               return _.Output.IsSome() ? ((Some<int>)_.Output).Content : 0;
            });
      }

      public override Task<Option<int>> SaveChangesAsync()
      {
         return SaveChangesAsync(None.Value);
      }

      public override Task<Option<int>> SaveChangesAsync(Option<CancellationToken> ctok)
      {
         return DataContext
            .Combine(ctok, true, CancellationToken.None)
            .Map(x => (DataContext: x.Item1, Ctok: x.Item2))
            .MapAsync(
            async x =>
            {
               if (x.DataContext.ExplicitSession.IsSome()) return 0;
               if (!x.DataContext.ImplicitSession.IsSome()) return 0;
               var _ = await x.DataContext.ImplicitSession
                  .IfMapAsync(
                     y => y.IsInTransaction,
                     async y =>
                     {
                        try
                        {
                           await y.CommitTransactionAsync(x.Ctok);
                           return 1;
                        }
                        catch (Exception)
                        {
                           await y.AbortTransactionAsync(x.Ctok);
                           return 0;
                        }
                     });
               return _.Output.IsSome() ? ((Some<int>)_.Output).Content : 0;
            });
      }

      public Task<Option<int>> DiscardChangesAsync()
      {
         return DiscardChangesAsync(None.Value);
      }

      public Task<Option<int>> DiscardChangesAsync(Option<CancellationToken> ctok)
      {
         return DataContext
            .Combine(ctok, true, CancellationToken.None)
            .Map(x => (DataContext: x.Item1, Ctok: x.Item2))
            .MapAsync(
               async x =>
               {
                  if (x.DataContext.ExplicitSession.IsSome()) return 0;
                  if (!x.DataContext.ImplicitSession.IsSome()) return 0;
                  var _ = await x.DataContext.ImplicitSession
                     .IfMapAsync(
                        y => y.IsInTransaction,
                        async y =>
                        {
                           try
                           {
                              await y.AbortTransactionAsync(x.Ctok);
                              return 1;
                           }
                           catch (Exception)
                           {
                              return 0;
                           }
                        });
                  return _.Output.IsSome() ? ((Some<int>)_.Output).Content : 0;
               });
      }

      #endregion

      #region Methods

      protected override Option<TRepository> InitRepository<TRepository, T>()
      {
         return new MongoRepository<T>(DataContext) as TRepository;
      }

      #endregion
   }
}