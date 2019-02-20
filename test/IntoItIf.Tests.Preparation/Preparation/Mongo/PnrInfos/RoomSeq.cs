namespace IntoItIf.Tests.Preparation.Preparation.Mongo.PnrInfos
{
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class RoomSeq
   {
      public int? No { get; set; }
      public int? Adult { get; set; }
      public int? Child { get; set; }
      public int? ChildAge1 { get; set; }
      public int? ChildAge2 { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }
   }
}