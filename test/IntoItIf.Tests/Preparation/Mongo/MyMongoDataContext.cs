namespace IntoItIf.Tests.Preparation.Mongo
{
   using MongoDb;
   using MongoDB.Driver;

   public class MyMongoDataContext : MongoDataContext
   {
      #region Constructors and Destructors

      public MyMongoDataContext(string connectionString, string dbName, FindOptions findOptions) : base(
         connectionString,
         dbName,
         findOptions)
      {
      }

      #endregion

      #region Methods

      protected override void OnModelCreating(MongoModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Contact>()
            .CreateMap(
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

      #endregion
   }
}