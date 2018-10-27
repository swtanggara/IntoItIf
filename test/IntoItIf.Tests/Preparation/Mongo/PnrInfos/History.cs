namespace IntoItIf.Tests.Preparation.Mongo.PnrInfos
{
   using System;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class History
   {
      #region Public Properties

      public string Id { get; set; }
      public string PnrId { get; set; }
      public string Event { get; set; }
      public string Description { get; set; }
      public DateTime? Created { get; set; }
      public string CreatedDisplay { get; set; }
      public string Username { get; set; }
      public string IpAddress { get; set; }
      public string Agent { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }

      #endregion
   }
}