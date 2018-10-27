namespace IntoItIf.Tests.Preparation.Mongo.PnrInfos
{
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class ElectronicTicket
   {
      #region Public Properties

      public string TicketNumber { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }

      #endregion
   }
}