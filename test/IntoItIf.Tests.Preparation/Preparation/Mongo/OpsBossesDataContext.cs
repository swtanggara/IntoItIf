namespace IntoItIf.Tests.Preparation.Preparation.Mongo
{
   using MongoDb;
   using MongoDB.Bson.Serialization.IdGenerators;
   using MongoDB.Driver;
   using PnrInfos;

   public class OpsBossesDataContext : MongoDataContext
   {
      #region Constructors and Destructors

      public OpsBossesDataContext(string connectionString, string dbName, FindOptions findOptions) : base(
         connectionString,
         dbName,
         findOptions)
      {
      }

      #endregion

      #region Methods

      protected override void OnModelCreating(MongoModelBuilder modelBuilder)
      {
         var consumer = modelBuilder.Entity<Consumer>();
         consumer.SetIndexes(x => x.Ascending(y => y.Code).IsUnique());
         consumer.CreateMap(
            x =>
            {
               x.AutoMap();
               x.MapCreator(y => new Consumer(y.Id, y.Code, y.Name, y.ClientId));
            });

         var pnr = modelBuilder.Entity<Pnr>();
         pnr.SetIndexes(
            x => x.Ascending(y => y.PnrId).IsUnique(),
            x => x.Ascending(y => y.Airline),
            x => x.Ascending(y => y.Agent),
            x => x.Ascending(y => y.BookedVia),
            x => x.Ascending(y => y.Company),
            x => x.Ascending(y => y.Created),
            x => x.Ascending(y => y.CustomerType),
            x => x.Ascending(y => y.IntlAirline),
            x => x.Ascending(y => y.IsInternational),
            x => x.Ascending(y => y.IssuedBy),
            x => x.Ascending(y => y.IssuedVia),
            x => x.Ascending(y => y.IssuedWithLg),
            x => x.Ascending(y => y.PassivePnrCode),
            x => x.Ascending(y => y.PassivePcc),
            x => x.Ascending(y => y.PassivePnrCode),
            x => x.Ascending(y => y.PaymentType),
            x => x.Ascending(y => y.Pcc),
            x => x.Ascending(y => y.PnrCode),
            x => x.Ascending(y => y.PnrNumericCode),
            x => x.Ascending(y => y.PnrReff),
            x => x.Ascending(y => y.PostStatus),
            x => x.Ascending(y => y.Prefix),
            x => x.Ascending(y => y.PrintStatus),
            x => x.Ascending(y => y.RequestToIssuedWithLG),
            x => x.Ascending(y => y.Reserved),
            x => x.Ascending(y => y.RsvNotifSent),
            x => x.Ascending(y => y.RoomTypeMapping),
            x => x.Ascending(y => y.Source),
            x => x.Ascending(y => y.SourcePnrCode),
            x => x.Ascending(y => y.Status),
            x => x.Ascending(y => y.StatusPassiveSegment),
            x => x.Ascending(y => y.Ticketed),
            x => x.Ascending(y => y.TimeLimit),
            x => x.Ascending(y => y.TransactionStatus),
            x => x.Ascending(y => y.UseInsurance),
            x => x.Ascending(y => y.Username)
         );
         pnr.CreateMap(
            x =>
            {
               x.AutoMap();
               x.MapIdMember(c => c.Id).SetIdGenerator(CombGuidGenerator.Instance);
            });
      }

      #endregion
   }
}