namespace IntoItIf.MongoDb
{
   using Base.UnitOfWork;
   using MongoDB.Driver;

   public interface IMongoUowDbTransaction : IUowDbTransaction<IClientSessionHandle>
   {
   }
}