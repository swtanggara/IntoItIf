namespace IntoItIf.Tests.Preparation.Mongo
{
   using Base.Domain.Options;
   using IntoItIf.MongoDb;
   using MongoDB.Driver;

   public class MyMongoDataContext : MongoDataContext
   {
      #region Constructors and Destructors

      public MyMongoDataContext(Option<string> connectionString, Option<string> dbName, Option<FindOptions> findOptions) : base(
         connectionString,
         dbName,
         findOptions)
      {
      }

      #endregion

      protected override void OnModelCreating(MongoModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Contact>().CreateMap(
            x =>
            {
               x.AutoMap();
               x.MapCreator(y => new Contact(y.Id, y.FirstName, y.LastName, y.Age));
            });

         modelBuilder.Entity<Contact>()
            .SetIndexes(
               x => x.Ascending(y => y.FirstName),
               x => x.Ascending(y => y.LastName),
               x => x.Ascending(y => y.Age));
      }
   }
}