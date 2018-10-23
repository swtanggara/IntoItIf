namespace IntoItIf.Tests.Preparation.Mongo
{
   using MongoDB.Bson.Serialization.Attributes;
   using MongoDB.Bson.Serialization.IdGenerators;

   public class Contact
   {
      [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
      public string Id { get; set; }

      public string FirstName { get; set; }
      public string LastName { get; set; }
      public double Age { get; set; }
   }
}