namespace IntoItIf.MongoDb
{
   using Base.Domain.Options;
   using MongoDB.Driver;

   public sealed class MongoUowScoped
   {
      private readonly Option<MongoUow> _uow;
      private readonly Option<IClientSessionHandle> _session;

      public MongoUowScoped(Option<MongoUow> uow, Option<IClientSessionHandle> session)
      {
         _uow = uow;
         _session = session;
      }

      public MongoRepository<T> SetOf<T>()
         where T : class
      {
         return _uow.MapFlatten(x => x.SetOf<T>()).ReduceOrDefault();
      }
   }
}