namespace IntoItIf.Tests.Preparation.Preparation.Mongo.PnrInfos
{
   using System;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class Activity
   {
      #region Public Properties

      public string Id { get; set; }
      public string PnrId { get; set; }
      public int? Operation { get; set; }
      public string Account { get; set; }
      public string Username { get; set; }
      public string Agent { get; set; }
      public DateTime? Created { get; set; }
      public DateTime? RunStart { get; set; }
      public DateTime? RunEnd { get; set; }
      public double? PrgNum { get; set; }
      public string PrgText { get; set; }
      public string Message { get; set; }
      public string StackTrace { get; set; }
      public int? Ercode { get; set; }
      public bool? Success { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }

      #endregion
   }
}