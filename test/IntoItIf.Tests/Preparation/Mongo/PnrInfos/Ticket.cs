namespace IntoItIf.Tests.Preparation.Mongo.PnrInfos
{
   using System;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class Ticket
   {
      public Guid Id { get; set; }
      public Guid PnrId { get; set; }
      public byte[] Pdf { get; set; }
      public DateTime? Created { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }
   }
}