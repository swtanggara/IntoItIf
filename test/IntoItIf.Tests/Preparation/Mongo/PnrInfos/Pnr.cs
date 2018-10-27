namespace IntoItIf.Tests.Preparation.Mongo.PnrInfos
{
   using System;
   using System.Collections.Generic;
   using Base.Domain.Entities;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class Pnr : BaseEntity<Pnr>
   {
      public Pnr()
      {
         Attachments = new List<object>();
         ElectronicTickets = new List<ElectronicTicket>();
         Histories = new List<History>();
         HotelRooms = new List<HotelRoom>();
         InsurancePolicies = new List<object>();
         OsiCollection = new List<OsiCollection>();
         Passengers = new List<Passenger>();
         Payments = new List<Payment>();
         PnrRates = new List<object>();
         Remarks = new List<object>();
         Segments = new List<Segment>();
         RawPayments = new List<Payment>();
      }
      #region Public Properties

      [BsonId]
      public ObjectId Id { get; set; }

      public string PnrId { get; set; }
      public int? Adult { get; set; }
      public int? Child { get; set; }
      public string Agent { get; set; }
      public string AgentName { get; set; }
      public int? Airline { get; set; }
      public IList<object> Attachments { get; set; }
      public int? BookedVia { get; set; }
      public decimal? Commission { get; set; }
      public string Company { get; set; }
      public string CompanyAddress { get; set; }
      public string CompanyName { get; set; }
      public string CompanyNpwp { get; set; }
      public Contact Contact { get; set; }
      public string ConversationId { get; set; }
      public string CorporateProfile { get; set; }
      public DateTime? Created { get; set; }
      public int? CustomerType { get; set; }
      public IList<ElectronicTicket> ElectronicTickets { get; set; }
      public string Error { get; set; }
      public decimal? FinalPrice { get; set; }
      public int? FlightType { get; set; }
      public int? GaCorporateType { get; set; }
      public decimal? GrandTotal { get; set; }
      public decimal? GrandTotalFare { get; set; }
      public decimal? GrandTotalPaid { get; set; }
      public decimal? TotalFare { get; set; }
      public decimal? TotalPaid { get; set; }
      public string Hash { get; set; }
      public IList<History> Histories { get; set; }
      public HotelInformation HotelInformation { get; set; }
      public IList<HotelRoom> HotelRooms { get; set; }
      public ElectronicTicket HotelVoucher { get; set; }
      public int? Infant { get; set; }
      public bool? InProgress { get; set; }
      public IList<object> InsurancePolicies { get; set; }
      public int? InsurancePoliciesPassengerPaid { get; set; }
      public int? InsurancePoliciesAgentPaid { get; set; }
      public IList<OsiCollection> OsiCollection { get; set; }
      public string FareType { get; set; }
      public string CorporateCode { get; set; }
      public string IntlAirline { get; set; }
      public bool? IsInternational { get; set; }
      public string IssuedBy { get; set; }
      public int? IssuedVia { get; set; }
      public string IssuedViaText { get; set; }
      public int? Item { get; set; }
      public Activity LastActivity { get; set; }
      public DateTime? LastSync { get; set; }
      public double? MerchantFeeAmount { get; set; }
      public DateTime? NotValid { get; set; }
      public IList<Passenger> Passengers { get; set; }
      public IList<Payment> Payments { get; set; }
      public int? PaymentType { get; set; }
      public string Pcc { get; set; }
      public string PnrCode { get; set; }
      public string PnrCodeDisplay { get; set; }
      public string PnrNumericCode { get; set; }
      public IList<object> PnrRates { get; set; }
      public Pnr PnrReference { get; set; }
      public string PnrReff { get; set; }
      public string PnrReffHistory { get; set; }
      public string PnrReffHistoryCode { get; set; }
      public string PnrReffHistoryDisplay { get; set; }
      public int? PnrReffHistoryStatus { get; set; }
      public int? PostStatus { get; set; }
      public int? PrintStatus { get; set; }
      public string BosInvoiceNo { get; set; }
      public string Reference { get; set; }
      public IList<object> Remarks { get; set; }
      public DateTime? Reserved { get; set; }
      public string RoomTypeMapping { get; set; }
      public int? RsvNotifSent { get; set; }
      public string SecurityToken { get; set; }
      public IList<Segment> Segments { get; set; }
      public DateTime? ServerTime { get; set; }
      public string VAInstruction { get; set; }
      public string FlagPayment { get; set; }
      public string TransactionStatus { get; set; }
      public string Source { get; set; }
      public string SourcePnrCode { get; set; }
      public int? Status { get; set; }
      public string StatusText { get; set; }
      public Ticket Ticket { get; set; }
      public DateTime? Ticketed { get; set; }
      public DateTime? TimeLimit { get; set; }
      public DateTime? TimeLimitShorted { get; set; }
      public string Prefix { get; set; }
      public string TourCode { get; set; }
      public string PricingCode { get; set; }
      public bool? UseInsurance { get; set; }
      public bool? UseMultiCurrency { get; set; }
      public string Username { get; set; }
      public int? StatusPassiveSegment { get; set; }
      public string PassivePnrCode { get; set; }
      public string PassivePcc { get; set; }
      public bool? BosAllowIssued { get; set; }
      public string BosAllowIssuedErrorMessage { get; set; }
      public string CaOtherCode { get; set; }
      public IList<Payment> RawPayments { get; set; }
      public double? Lat { get; set; }
      public double? Lon { get; set; }
      public double? Radius { get; set; }
      public string PaymentMidtransStatus { get; set; }
      public string PaymentMidtransReff { get; set; }
      public decimal? PaymentLinkFeeAmount { get; set; }
      public bool? IssuedWithLg { get; set; }
      public bool? RequestToIssuedWithLG { get; set; }

      [BsonExtraElements]
      public BsonDocument CatchAll { get; set; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         return new List<object>();
      }

      #endregion
   }
}