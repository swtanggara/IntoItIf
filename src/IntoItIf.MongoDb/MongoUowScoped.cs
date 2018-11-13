namespace IntoItIf.MongoDb
{
   using Base.Domain.Options;
   using MongoDB.Driver;

   public sealed class MongoUowScoped
   {
      private readonly Option<MongoUow> _uow;

      public MongoUowScoped(Option<MongoUow> uow, Option<IClientSessionHandle> session)
      {
         _uow = uow;
         Session = session.ReduceOrDefault();
      }

      public IClientSessionHandle Session { get; }

      public IMongoRepository<T> SetOf<T>()
         where T : class
      {
         return _uow.MapFlatten(x => x.SetOf<T>()).ReduceOrDefault();
      }
   }
}