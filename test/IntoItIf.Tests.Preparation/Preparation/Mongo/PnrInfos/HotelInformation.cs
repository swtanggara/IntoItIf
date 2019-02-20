namespace IntoItIf.Tests.Preparation.Preparation.Mongo.PnrInfos
{
   using System;
   using System.Collections.Generic;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class HotelInformation
   {
      #region Public Properties

      public Guid PnrId { get; set; }
      public string Source { get; set; }
      public string HotelCode { get; set; }
      public string HotelName { get; set; }
      public string CityCode { get; set; }
      public string CityName { get; set; }
      public string CountryCode { get; set; }
      public string CountryName { get; set; }
      public DateTime? CheckinDate { get; set; }
      public DateTime? CheckoutDate { get; set; }
      public string RoomCode { get; set; }
      public int? NumberofRoom { get; set; }
      public string RoomName { get; set; }
      public string RoomId { get; set; }
      public string RoomCategoryName { get; set; }
      public string Availability { get; set; }
      public string Meal { get; set; }
      public int? Nights { get; set; }
      public decimal? TotalRate { get; set; }
      public string PaxCountry { get; set; }
      public DateTime? Modified { get; set; }
      public string InternalCode { get; set; }
      public string ImageUrl { get; set; }
      public string Remark { get; set; }

      public List<string> Notes { get; set; }

      //public string InternalCode { get; set; }
      public string Currency { get; set; }
      public string EssentialInformation { get; set; }
      public string HotelRating { get; set; }
      public string Address { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }

      #endregion
   }
}