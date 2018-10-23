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
         modelBuilder.Entity<People>().SetIndexes(x => x.Ascending(y => y.IdNumber).IsUnique());
         modelBuilder.Entity<People>().SetIndexes(x => x.Ascending(y => y.NickName).IsUnique());
      }
   }
}