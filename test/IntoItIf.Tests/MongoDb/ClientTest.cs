namespace IntoItIf.Tests.MongoDb
{
   using System;
   using Base.Helpers;
   using IntoItIf.MongoDb;
   using MongoDB.Driver;
   using Preparation.Mongo;
   using Xunit;
   using Xunit.Abstractions;

   public class ClientTest
   {
      private readonly ITestOutputHelper _output;
      private const string ConnString = "mongodb://eq:VS8vNXA5T4Kywh9H@192.168.10.7:27017/?authSource=admin";
      private const string DbName = "OpsigoXX";
      private static readonly FindOptions FindOptions = new FindOptions { MaxTime = TimeSpan.FromSeconds(10) };
      private readonly MyMongoDataContext _context;

      public ClientTest(ITestOutputHelper output)
      {
         _output = output;
         _context = new MyMongoDataContext(ConnString, DbName, FindOptions);
      }

      [Fact]
      public void ShouldConnect()
      {
         var jadaStevens = new People
         {
            FirstName = "Jada",
            LastName = "Stevens",
         };

         var uow = new MongoUow(_context);
         var result = uow.SaveChangesForScopedAsync(
               async x =>
               {
                  await x.SetOf<People>().AddAsync(jadaStevens);
                  return await x.SetOf<People>().GetListAsync(y => y, y => y.FirstName == "Jada");
               })
            .ToSync();
      }
   }
}