namespace IntoItIf.Tests.Preparation.Mongo.PnrInfos
{
   using System;
   using System.Collections.Generic;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class HotelRoom
   {
      public Guid Id { get; set; }
      public int? Index { get; set; }
      public string RoomCode { get; set; }
      public string RoomName { get; set; }
      public bool? ExtraBed { get; set; }
      public int? NumberOfRooms { get; set; }
      public int? NumberOfExtraBeds { get; set; }
      public int? NumberOfCots { get; set; }

      public string CategoryId { get; set; }

      public ICollection<RoomSeq> RoomSeqs { get; set; }
      public ICollection<RoomRateInfo> RoomRateInfos { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }
   }
}