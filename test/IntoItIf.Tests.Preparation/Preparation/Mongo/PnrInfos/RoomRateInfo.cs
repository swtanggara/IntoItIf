namespace IntoItIf.Tests.Preparation.Preparation.Mongo.PnrInfos
{
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class RoomRateInfo
   {
      public int? Index { get; set; }
      public string Price { get; set; }
      public decimal? TotalPrice { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }
   }
}