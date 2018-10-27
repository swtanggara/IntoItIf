namespace IntoItIf.Tests.Preparation.Mongo.PnrInfos
{
   using System.Collections.Generic;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class Payment
   {
      #region Public Properties

      public string Id { get; set; }
      public string PolicyId { get; set; }
      public int? Airline { get; set; }
      public string Code { get; set; }
      public string Title { get; set; }
      public decimal? Amount { get; set; }
      public string Currency { get; set; }
      public decimal? ForeignAmount { get; set; }
      public string ForeignCurrency { get; set; }
      public int? Order { get; set; }
      public decimal? CancellationCharge { get; set; }
      public IList<string> FareCodes { get; set; }
      public string FareRules { get; set; }
      public string EquivFareRules { get; set; }
      public string EquivCurr { get; set; }
      public string PnrId { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }

      #endregion
   }
}