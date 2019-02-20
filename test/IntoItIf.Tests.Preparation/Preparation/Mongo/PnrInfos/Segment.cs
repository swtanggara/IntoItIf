namespace IntoItIf.Tests.Preparation.Preparation.Mongo.PnrInfos
{
   using System;
   using System.Collections.Generic;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class Segment
   {
      #region Public Properties

      public string Id { get; set; }
      public string FlightNumber { get; set; }
      public string PnrId { get; set; }
      public string CarrierCode { get; set; }
      public DateTime? DepartureDate { get; set; }
      public DateTime? ArrivalDate { get; set; }
      public string OperatingAirline { get; set; }
      public string OperatingFlightNumber { get; set; }
      public string MarketingAirline { get; set; }
      public string ImageUrl { get; set; }
      public string AirlineResCode { get; set; }
      public string Origin { get; set; }
      public string Destination { get; set; }
      public string Depart { get; set; }
      public string Arrive { get; set; }
      public int? Num { get; set; }
      public int? Seq { get; set; }
      public string Class { get; set; }
      public string ClassCategory { get; set; }
      public string ClassInfo { get; set; }
      public string ClassHash { get; set; }
      public DateTime? Modified { get; set; }
      public string Hash { get; set; }
      public int? Airline { get; set; }
      public string OriginName { get; set; }
      public string DestinationName { get; set; }
      public string OriginAirportName { get; set; }
      public string DestinationAirportName { get; set; }
      public string ApiRefKey { get; set; }
      public string FareBasisCode { get; set; }
      public int? AirMilesFlown { get; set; }
      public string DepartTerminal { get; set; }
      public string ArriveTerminal { get; set; }
      public string AirlineName { get; set; }
      public string AirEquipType { get; set; }
      public IList<string> Meals { get; set; }
      public string CabinClass { get; set; }
      public string CabinClassDisplay { get; set; }
      public int? Provider { get; set; }
      public string SegmentSellKey { get; set; }
      public string CarrierName { get; set; }
      public string SupplierLocatorCode { get; set; }
      public string Baggage { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }

      #endregion
   }
}