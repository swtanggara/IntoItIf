namespace IntoItIf.MongoDb
{
   using MongoDB.Driver;

   public sealed class MongoUowScoped
   {
      private readonly MongoUow _uow;

      public MongoUowScoped(MongoUow uow, IClientSessionHandle session)
      {
         _uow = uow;
         Session = session;
      }

      public IClientSessionHandle Session { get; }

      public IMongoRepository<T> SetOf<T>()
         where T : class, IMongoEntity
      {
         return _uow.SetOf<T>();
      }
   }
}