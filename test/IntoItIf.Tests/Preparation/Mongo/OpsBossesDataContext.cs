namespace IntoItIf.Tests.Preparation.Mongo
{
   using Base.Domain.Options;
   using IntoItIf.MongoDb;
   using MongoDB.Driver;

   public class OpsBossesDataContext : MongoDataContext
   {
      #region Constructors and Destructors

      public OpsBossesDataContext(Option<string> connectionString, Option<string> dbName, Option<FindOptions> findOptions) : base(
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
         consumer.CreateMap(
            x =>
            {
               x.AutoMap();
               x.MapCreator(y => new Consumer(y.Id, y.Code, y.Name, y.ClientId));
            });
         consumer.SetIndexes(x => x.Ascending(y => y.Code).IsUnique());
      }

      #endregion
   }
}