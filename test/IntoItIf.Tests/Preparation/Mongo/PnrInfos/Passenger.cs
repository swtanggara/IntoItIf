namespace IntoItIf.Tests.Preparation.Mongo.PnrInfos
{
   using System;
   using System.Collections.Generic;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class Passenger
   {
      #region Public Properties

      public string Address { get; set; }
      public int? AdultAssoc { get; set; }
      public DateTime? BirthDate { get; set; }
      public string BirthDateView { get; set; }
      public int? BrdDay { get; set; }
      public int? BrdMonth { get; set; }
      public int? BrdYear { get; set; }
      public string City { get; set; }
      public string CompanyName { get; set; }
      public string Display { get; set; }
      public string Email { get; set; }
      public string EmployeeId { get; set; }
      public int? ExpDay { get; set; }
      public int? ExpMonth { get; set; }
      public int? ExpYear { get; set; }
      public string BirthPlace { get; set; }
      public string FirstName { get; set; }
      public string FullName { get; set; }
      public string HomePhone { get; set; }
      public string OtherPhone { get; set; }
      public string Id { get; set; }
      public string IdNumberType { get; set; }
      public string IdNumber { get; set; }
      public bool? IsSeniorPax { get; set; }
      public string SeatNumber { get; set; }
      public IList<object> IncomingSsrs { get; set; }
      public int? Index { get; set; }
      public int? IndexFromGds { get; set; }
      public int? InsuredPersonType { get; set; }
      public IList<object> IntlSsrs { get; set; }
      public string LastName { get; set; }
      public string MiddleName { get; set; }
      public string MobilePhone { get; set; }
      public string Nationality { get; set; }
      public IList<object> OutgoingSsrs { get; set; }
      public DateTime? PassportExpire { get; set; }
      public string PassportNumber { get; set; }
      public string PassportFirstName { get; set; }
      public string PassportMidleName { get; set; }
      public string PassportLastName { get; set; }
      public string PassportOrigin { get; set; }
      public string PostalCode { get; set; }
      public string Province { get; set; }
      public string FrequentPaxNumber { get; set; }
      public string Remark1 { get; set; }
      public string Remark2 { get; set; }
      public string Remark3 { get; set; }
      public string Remark4 { get; set; }
      public string Remark5 { get; set; }
      public string Remark6 { get; set; }
      public string VisaNumber { get; set; }
      public DateTime? VisaDateIssued { get; set; }
      public string VisaPlaceOfIssued { get; set; }
      public string VisaCountryApplied { get; set; }
      public IList<object> FrequentFlyers { get; set; }
      public bool? AnyRemarks { get; set; }
      public IList<object> Seats { get; set; }
      public int? Sex { get; set; }
      public string Ssr { get; set; }
      public string Ticket { get; set; }
      public string Age { get; set; }
      public string Title { get; set; }
      public int? Type { get; set; }
      public string Country { get; set; }
      public string CostCenter { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }

      #endregion
   }
}