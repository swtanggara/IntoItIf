namespace IntoItIf.Tests.Preparation.Mongo.PnrInfos
{
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class Contact
   {
      #region Public Properties

      public string PnrId { get; set; }
      public string Title { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Email { get; set; }
      public string EmailFr { get; set; }
      public string MobilePhone { get; set; }
      public string HomePhone { get; set; }
      public string ContactAgent { get; set; }
      public string FullName { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }

      #endregion
   }
}