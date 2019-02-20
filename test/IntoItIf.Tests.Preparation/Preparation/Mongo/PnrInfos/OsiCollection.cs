namespace IntoItIf.Tests.Preparation.Preparation.Mongo.PnrInfos
{
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class OsiCollection
   {
      #region Public Properties

      public string Id { get; set; }
      public string PnrId { get; set; }
      public string CarrierCode { get; set; }
      public string Text { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }

      #endregion
   }
}