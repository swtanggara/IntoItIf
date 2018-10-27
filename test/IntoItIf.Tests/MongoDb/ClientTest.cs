namespace IntoItIf.Tests.MongoDb
{
   using System;
   using System.Collections.Generic;
   using System.Threading.Tasks;
   using Base.Domain.Options;
   using Base.UnitOfWork;
   using IntoItIf.MongoDb;
   using MongoDB.Driver;
   using Newtonsoft.Json;
   using Preparation.Mongo;
   using Preparation.Mongo.PnrInfos;
   using Shouldly;
   using Xunit;
   using Xunit.Abstractions;

   public class ClientTest
   {
      private readonly ITestOutputHelper _output;
      //private const string ConnString = "mongodb://sa:UafVaf2KMf3roLSC@localhost:27017/OpsBosses?authSource=admin&retryWrites=true&replicaSet=rs0";
      private const string ConnString = "mongodb://eq:VS8vNXA5T4Kywh9H@192.168.10.7:27017/admin";
      //private const string ConnString =
      //   "mongodb://sa:UafVaf2KMf3roLSC@intoitifmongodb-shard-00-00-5qpey.mongodb.net:27017,intoitifmongodb-shard-00-01-5qpey.mongodb.net:27017,intoitifmongodb-shard-00-02-5qpey.mongodb.net:27017/test?ssl=true&replicaSet=IntoItIfMongoDb-shard-0&authSource=admin&retryWrites=true";
      private const string DbName = "OpsBosses";
      private static readonly FindOptions FindOptions = new FindOptions { MaxTime = TimeSpan.FromSeconds(10) };
      private readonly OpsBossesDataContext _context;

      public ClientTest(ITestOutputHelper output)
      {
         _output = output;
         _context = new OpsBossesDataContext(ConnString, DbName, null);
      }

      [Fact]
      public async Task ShouldConsumerConnect()
      {
         ITransactionUow uow = new MongoUow(_context);
         const string json = "{\"Code\":\"GN\",\"Name\":\"Golden Nusa\",\"ClientId\":\"client-cre-opsbos-gn\"}";
         var args = JsonConvert.DeserializeObject<Consumer>(json);
         var result = await uow.GetRepository<MongoRepository<Consumer>, Consumer>()
            .Combine(args.ToOption())
            .Map(x => (Repo: x.Item1, Args: x.Item2))
            .MapFlattenAsync(
               async x =>
               {
                  using (var trx = uow.GetDbTransaction<OpsBossesDataContext>())
                  {
                     try
                     {
                        var inResult = await x.Repo.AddAsync(x.Args);
                        if (inResult.IsFail()) return inResult;
                        trx.Commit();
                        return inResult;
                     }
                     catch (Exception)
                     {
                        trx.Rollback();
                        throw;
                     }
                  }
               });
         result.ReduceOrDefault().ShouldNotBeNull();
      }

      [Fact]
      public async Task ShouldPnrConnect()
      {
         ITransactionUow uow = new MongoUow(_context);
         const string jsonPnr = "{\n  \"PnrId\": \"65ebf98e-2fa8-4c90-af67-d177f4523da7\",\n  \"Adult\": 2,\n  \"Child\": 1,\n  \"Agent\": \"COM00001\",\n  \"AgentName\": \"RSP (REKA SINERGI PRATAMA PT)\",\n  \"Airline\": 22,\n  \"Attachments\": [],\n  \"BookedVia\": 0,\n  \"Commission\": 0,\n  \"Company\": \"COM00353\",\n  \"CompanyAddress\": \"PONDOK INDAH OFFICE TOWER 3 LT.12\",\n  \"CompanyName\": \"REKA SINERGI PRATAMA\",\n  \"CompanyNpwp\": \"12.345.678.9-999.999\",\n  \"Contact\": {\n    \"PnrId\": \"18a8dace-17ca-4b5c-8957-a6e3e8a1ccbd\",\n    \"Title\": \"MR\",\n    \"FirstName\": \"ANGGARA\",\n    \"LastName\": \"SUWARTANA\",\n    \"Email\": \"support@gmail.com\",\n    \"EmailFr\": \"\",\n    \"MobilePhone\": \"6281219912696\",\n    \"HomePhone\": \"6281219912696\",\n    \"ContactAgent\": \"\",\n    \"FullName\": \"ANGGARA SUWARTANA\"\n  },\n  \"ConversationId\": \"\",\n  \"CorporateProfile\": \"\",\n  \"Created\": \"2018-10-27T00:23:40.7167233\",\n  \"CustomerType\": 2,\n  \"ElectronicTickets\": [],\n  \"Error\": \"\",\n  \"FinalPrice\": 8349300,\n  \"FlightType\": 1,\n  \"GaCorporateType\": 0,\n  \"GrandTotal\": 8349300,\n  \"GrandTotalFare\": 8349300,\n  \"GrandTotalPaid\": 8349300,\n  \"TotalFare\": 8349300,\n  \"TotalPaid\": 8349300,\n  \"Hash\": \"LK1L6PSOHM\",\n  \"Histories\": [\n    {\n      \"Id\": \"fe3dc118-f905-4967-b10a-6ba40242bbb6\",\n      \"PnrId\": \"18a8dace-17ca-4b5c-8957-a6e3e8a1ccbd\",\n      \"Event\": \"Book\",\n      \"Description\": \"Reservation booked \\nPNR Code 7WRTZE\\n\",\n      \"Created\": \"2018-10-27T00:25:04.9026616\",\n      \"CreatedDisplay\": \"25 minutes ago\",\n      \"Username\": \"swtanggara\",\n      \"IpAddress\": \"\",\n      \"Agent\": \"RSP (REKA SINERGI PRATAMA PT)\"\n    },\n    {\n      \"Id\": \"f98bed23-ef63-4d01-a1be-a65bc8b1e265\",\n      \"PnrId\": \"18a8dace-17ca-4b5c-8957-a6e3e8a1ccbd\",\n      \"Event\": \"Create\",\n      \"Description\": \"Creating new reservation with Process ID \'LK1L6PSOHM\'\\nAdd Passenger:\\nMR MOSES MUSA\\nMS MOHAMMED SALAH\\nMSTR BUDOIN BUDI\\nMSTR ANELKA NIKOLAS\\nAdd Segment:\\nCGKKUL 31-Oct-2018 11:10 710. Class: Q\\n\\r\\nKULSIN 31-Oct-2018 15:30 605. Class: Q\\n\\r\\nSINKUL 07-Nov-2018 13:40 606. Class: Q\\n\\r\\nKULCGK 07-Nov-2018 16:25 723. Class: Q\\n\\r\\n\",\n      \"Created\": \"2018-10-27T00:23:41.5448317\",\n      \"CreatedDisplay\": \"27 minutes ago\",\n      \"Username\": \"swtanggara\",\n      \"IpAddress\": \"\",\n      \"Agent\": \"RSP (REKA SINERGI PRATAMA PT)\"\n    }\n  ],\n  \"HotelInformation\": null,\n  \"HotelRooms\": [],\n  \"HotelVoucher\": null,\n  \"Infant\": 1,\n  \"InProgress\": false,\n  \"InsurancePolicies\": [],\n  \"InsurancePoliciesPassengerPaid\": 0,\n  \"InsurancePoliciesAgentPaid\": 0,\n  \"OsiCollection\": [\n    {\n      \"Id\": \"c9cd315c-0db4-4873-af2b-2c5f1241ce7c\",\n      \"PnrId\": \"00000000-0000-0000-0000-000000000000\",\n      \"CarrierCode\": \"MH\",\n      \"Text\": \"BLA\"\n    }\n  ],\n  \"FareType\": \"Market\",\n  \"CorporateCode\": \"\",\n  \"IntlAirline\": \"MH\",\n  \"IsInternational\": false,\n  \"IssuedBy\": \"\",\n  \"IssuedVia\": 0,\n  \"IssuedViaText\": null,\n  \"Item\": 0,\n  \"LastActivity\": {\n    \"Id\": \"8949702d-05b2-4105-bb13-20e2812305f2\",\n    \"PnrId\": \"18a8dace-17ca-4b5c-8957-a6e3e8a1ccbd\",\n    \"Operation\": 2,\n    \"Account\": \"Universal API/uAPI8727325326-f2c0c608\",\n    \"Username\": \"swtanggara\",\n    \"Agent\": \"COM00001\",\n    \"Created\": \"2018-10-27T00:23:44.1385445\",\n    \"RunStart\": \"2018-10-27T00:23:44.2166697\",\n    \"RunEnd\": \"2018-10-27T00:25:05.9025928\",\n    \"PrgNum\": 100,\n    \"PrgText\": \"Completed\",\n    \"Message\": \"Completed\",\n    \"StackTrace\": \"\",\n    \"Ercode\": 0,\n    \"Success\": true\n  },\n  \"LastSync\": \"2018-10-27T00:23:44.1385445\",\n  \"MerchantFeeAmount\": 0,\n  \"NotValid\": \"0001-01-01T00:00:00\",\n  \"Passengers\": [\n    {\n      \"Address\": \"\",\n      \"AdultAssoc\": 0,\n      \"BirthDate\": \"2008-01-01T00:00:00\",\n      \"BirthDateView\": \"\",\n      \"BrdDay\": 0,\n      \"BrdMonth\": 0,\n      \"BrdYear\": 0,\n      \"City\": \"\",\n      \"CompanyName\": \"\",\n      \"Display\": \"BUDI/BUDOIN MSTR\",\n      \"Email\": \"\",\n      \"EmployeeId\": \"\",\n      \"ExpDay\": 0,\n      \"ExpMonth\": 0,\n      \"ExpYear\": 0,\n      \"BirthPlace\": \"\",\n      \"FirstName\": \"BUDOIN\",\n      \"FullName\": \"BUDOIN BUDI\",\n      \"HomePhone\": \"\",\n      \"OtherPhone\": \"\",\n      \"Id\": \"8fccc400-f238-4d69-81ef-06d5356ea89c\",\n      \"IdNumberType\": \"\",\n      \"IdNumber\": \"\",\n      \"IsSeniorPax\": false,\n      \"SeatNumber\": \"\",\n      \"IncomingSsrs\": [],\n      \"Index\": 3,\n      \"IndexFromGds\": 0,\n      \"InsuredPersonType\": 0,\n      \"IntlSsrs\": [],\n      \"LastName\": \"BUDI\",\n      \"MiddleName\": \"\",\n      \"MobilePhone\": \"\",\n      \"Nationality\": \"ID\",\n      \"OutgoingSsrs\": [],\n      \"PassportExpire\": \"2022-01-01T00:00:00\",\n      \"PassportNumber\": \"1235132132\",\n      \"PassportFirstName\": \"\",\n      \"PassportMidleName\": \"\",\n      \"PassportLastName\": \"\",\n      \"PassportOrigin\": \"ID\",\n      \"PaxSeats\": [],\n      \"PaxSsrs\": [],\n      \"PostalCode\": \"\",\n      \"Province\": \"\",\n      \"FrequentPaxNumber\": \"\",\n      \"Remark1\": \"\",\n      \"Remark2\": \"\",\n      \"Remark3\": \"\",\n      \"Remark4\": \"\",\n      \"Remark5\": \"\",\n      \"Remark6\": \"\",\n      \"VisaNumber\": \"\",\n      \"VisaDateIssued\": \"2018-01-01T00:00:00\",\n      \"VisaPlaceOfIssued\": \"\",\n      \"VisaCountryApplied\": \"\",\n      \"FrequentFlyers\": [],\n      \"AnyRemarks\": false,\n      \"Seats\": [],\n      \"Sex\": 0,\n      \"Ssr\": \"\",\n      \"Ticket\": \"\",\n      \"Age\": \"10\",\n      \"Title\": \"MSTR\",\n      \"Type\": 2,\n      \"PassengerCorporate\": null,\n      \"Country\": \"\",\n      \"CostCenter\": \"\"\n    }\n  ],\n  \"Payments\": [\n    {\n      \"Id\": \"b2b5d213-2d36-4657-a870-7fcbe444c92d\",\n      \"PolicyId\": \"00000000-0000-0000-0000-000000000000\",\n      \"Airline\": 0,\n      \"Code\": \"BASE_FARE\",\n      \"Title\": \"Base Fare\",\n      \"Amount\": 6018000,\n      \"Currency\": \"IDR\",\n      \"ForeignAmount\": 0,\n      \"ForeignCurrency\": \"\",\n      \"Order\": 1,\n      \"CancellationCharge\": 0,\n      \"FareCodes\": [\n        \"QBC1YID\",\n        \"QBC1YIDC\",\n        \"QBC1YIDI\"\n      ],\n      \"FareRules\": \"4,080,000\",\n      \"EquivFareRules\": \"\",\n      \"EquivCurr\": \"\",\n      \"PurchaseText\": null,\n      \"Rules\": null,\n      \"PnrId\": \"00000000-0000-0000-0000-000000000000\"\n    }\n  ],\n  \"PaymentType\": 0,\n  \"Pcc\": \"\",\n  \"PnrCode\": \"7WRTZE\",\n  \"PnrCodeDisplay\": \"\",\n  \"PnrNumericCode\": \"\",\n  \"PnrRates\": [],\n  \"PnrReference\": null,\n  \"PnrReff\": \"\",\n  \"PnrReffHistory\": \"\",\n  \"PnrReffHistoryCode\": \"\",\n  \"PnrReffHistoryDisplay\": \"\",\n  \"PnrReffHistoryStatus\": 0,\n  \"PostStatus\": 0,\n  \"PrintStatus\": 0,\n  \"BosInvoiceNo\": \"-\",\n  \"Reference\": \"\",\n  \"Remarks\": [],\n  \"Reserved\": \"2018-10-27T00:25:03.68388\",\n  \"RoomTypeMapping\": \"\",\n  \"RsvNotifSent\": 0,\n  \"SecurityToken\": \"\",\n  \"Segments\": [\n    {\n      \"Id\": \"d93914e6-e9b9-45d6-b707-2611e2f858d7\",\n      \"FlightNumber\": \"710\",\n      \"PnrId\": \"18a8dace-17ca-4b5c-8957-a6e3e8a1ccbd\",\n      \"CarrierCode\": \"\",\n      \"DepartureDate\": \"2018-10-31T00:00:00\",\n      \"ArrivalDate\": \"2018-10-31T00:00:00\",\n      \"OperatingAirline\": \"\",\n      \"OperatingFlightNumber\": \"\",\n      \"MarketingAirline\": \"MH\",\n      \"ImageUrl\": \"http://portalvhds11000v9mfhk0k.blob.core.windows.net/airline/MH-mail.png\",\n      \"AirlineResCode\": \"\",\n      \"Origin\": \"CGK\",\n      \"Destination\": \"KUL\",\n      \"Depart\": \"11:10:00\",\n      \"Arrive\": \"14:15:00\",\n      \"Num\": 0,\n      \"Seq\": 0,\n      \"Class\": \"Q\",\n      \"ClassCategory\": \"\",\n      \"ClassInfo\": \"\",\n      \"ClassHash\": \"\",\n      \"Modified\": \"2018-10-27T00:23:40.7479868\",\n      \"Hash\": \"CGKKUL 710_Q\",\n      \"Airline\": 0,\n      \"OriginName\": \"Jakarta\",\n      \"DestinationName\": \"Kuala Lumpur\",\n      \"OriginAirportName\": \"Soekarno Hatta\",\n      \"DestinationAirportName\": \"Kuala Lumpur International Airport\",\n      \"ApiRefKey\": \"\",\n      \"FareBasisCode\": \"\",\n      \"AirMilesFlown\": 0,\n      \"DepartTerminal\": \"\",\n      \"ArriveTerminal\": \"\",\n      \"AirlineName\": \"\",\n      \"AirEquipType\": \"\",\n      \"Meals\": null,\n      \"CabinClass\": \"Economy\",\n      \"CabinClassDisplay\": \"Economy\",\n      \"Provider\": 22,\n      \"SegmentSellKey\": \"{\\\"Key\\\":\\\"BNBXua7Q2BKAWIpbAAAAAA==\\\",\\\"Group\\\":\\\"0\\\",\\\"DepartureTimeZone\\\":\\\"000+07:00\\\",\\\"ArrivalTimeZone\\\":\\\"000+08:00\\\",\\\"Equipment\\\":\\\"738\\\",\\\"AvailabilitySource\\\":\\\"S\\\",\\\"AvailabilityDisplayType\\\":\\\"Fare Shop/Optimal Shop\\\",\\\"IsConnection\\\":\\\"True\\\",\\\"FlightTime\\\":\\\"125\\\",\\\"Distance\\\":\\\"725\\\",\\\"TravelTime\\\":null,\\\"ChangeOfPlane\\\":\\\"False\\\",\\\"PolledAvailabilityOption\\\":\\\"Polled avail used\\\",\\\"LinkAvailability\\\":\\\"True\\\",\\\"ParticipantLevel\\\":\\\"Secure Sell\\\",\\\"OptionalServicesIndicator\\\":\\\"False\\\",\\\"ETicketability\\\":\\\"Yes\\\",\\\"FareBasisCode\\\":\\\"QBC1YID\\\",\\\"FareRuleKeys\\\":\\\"[\\\\\\\"gws-eJxNTtEOwjAI/Jjl3gFd171VW7c1xiZGfeiL//8Zg1aTkcAdOTgIIQixZ5IpHGLAd3hsKJ8IFIjmKxc4PzqwNhVE5PG8Rq45oTucTClN7chtLs7xrIBFFu6SBWqrcb2bVTNWZNhhGNUN/MltU1ou6Z2TsB6eiUbvfiJN0E93X/YrNg==\\\\\\\",\\\\\\\"gws-eJxNTssOAjEI/JjN3AF1t3trZF+NkcQYD734/58htJpIAjNkYCDnLMSJSab8FwPew/2AvRQwiOezGC5JEtibCiJKeFyVa1nQHU6hWFM7SpvTWc8O2GTjLkWgtqr7LayasaMgDiOob+BH1sOpqVlZhP3wHBvjV6QJ/ukHX5IrOg==\\\\\\\",\\\\\\\"gws-eJxNTkEKAjEMfMwy9yR1d7u3Yte6RQyIeOjF/z/DpFUwkMyQCTNJKQlxZJI1/dWE93Q/oK8MKMT6WRVLnMHGG4go4nHO3OqOYRBc0a4ODP0ub/lkgCKFh+SF1me+3tyK3NggwHPRVwT8yOUwqlVL3YUteCOa4/IVaYU9+gE1MysC\\\\\\\"]\\\"}\",\n      \"CarrierName\": \"Malaysia Airlines (MH)\",\n      \"SupplierLocatorCode\": \"MO8J44\",\n      \"Baggage\": \"\"\n    }\n  ],\n  \"ServerTime\": \"2018-10-27T00:50:44.5208946+07:00\",\n  \"VAInstruction\": \"\",\n  \"FlagPayment\": \"\",\n  \"TransactionStatus\": \"\",\n  \"Source\": \"\",\n  \"SourcePnrCode\": \"\",\n  \"Status\": 1,\n  \"StatusText\": \"\",\n  \"Ticket\": null,\n  \"Ticketed\": \"2018-10-27T00:50:44.5238864+07:00\",\n  \"TimeLimit\": \"2018-10-27T00:50:44.5238864+07:00\",\n  \"TimeLimitShorted\": \"2018-10-27T00:50:44.5238864+07:00\",\n  \"Prefix\": \"\",\n  \"TourCode\": \"\",\n  \"PricingCode\": \"\",\n  \"UseInsurance\": false,\n  \"UseMultiCurrency\": false,\n  \"Username\": \"swtanggara\",\n  \"Promo\": null,\n  \"StatusPassiveSegment\": 0,\n  \"PassivePnrCode\": \"\",\n  \"PassivePcc\": \"\",\n  \"BosAllowIssued\": false,\n  \"BosAllowIssuedErrorMessage\": \"\",\n  \"HotelsJobRequest\": null,\n  \"HtlbedsRq\": null,\n  \"CaOtherCode\": \"\",\n  \"RawPayments\": [\n    {\n      \"Id\": \"b2b5d213-2d36-4657-a870-7fcbe444c92d\",\n      \"PolicyId\": \"00000000-0000-0000-0000-000000000000\",\n      \"Airline\": 0,\n      \"Code\": \"BASE_FARE_QBC1YID_ADT\",\n      \"Title\": \"\",\n      \"Amount\": 4080000,\n      \"Currency\": \"IDR\",\n      \"ForeignAmount\": 0,\n      \"ForeignCurrency\": \"\",\n      \"Order\": 0,\n      \"CancellationCharge\": 0,\n      \"FareCodes\": \"\",\n      \"FareRules\": \"\",\n      \"EquivFareRules\": \"\",\n      \"EquivCurr\": \"\",\n      \"PurchaseText\": null,\n      \"Rules\": null,\n      \"PnrId\": \"18a8dace-17ca-4b5c-8957-a6e3e8a1ccbd\"\n    }\n  ],\n  \"Lat\": 0,\n  \"Lon\": 0,\n  \"Radius\": 0,\n  \"PaymentMidtransStatus\": \"\",\n  \"PaymentMidtransReff\": \"\",\n  \"PaymentLinkFeeAmount\": 0,\n  \"IssuedWithLg\": false,\n  \"RequestToIssuedWithLG\": false\n}";
         var args = JsonConvert.DeserializeObject<Pnr>(jsonPnr);
         var result = await uow.GetRepository<MongoRepository<Pnr>, Pnr>()
            .Combine(args.ToOption())
            .Map(x => (Repo: x.Item1, Args: x.Item2))
            .MapAsync(
               async x =>
               {
                  using (var trx = uow.GetDbTransaction<OpsBossesDataContext>())
                  {
                     try
                     {
                        await x.Repo.AddAsync(x.Args);
                        trx.Commit();
                        return x.Args;
                     }
                     catch (Exception)
                     {
                        trx.Rollback();
                        throw;
                     }
                  }
               });
         result.ReduceOrDefault().ShouldNotBeNull();
      }
   }
}