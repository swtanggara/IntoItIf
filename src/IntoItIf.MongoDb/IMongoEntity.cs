namespace IntoItIf.MongoDb
{
   using Base.Domain.Entities;
   using MongoDB.Bson;

   public interface IMongoEntity : IEntity<ObjectId>
   {
   }
}