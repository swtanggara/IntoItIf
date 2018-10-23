namespace IntoItIf.Tests.MongoDb
{
   using System;
   using Base.Domain.Options;
   using MongoDB.Bson;
   using MongoDB.Driver;
   using Preparation.Mongo;
   using Shouldly;
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
         var jadaStevens = new Contact
         {
            FirstName = "Jada",
            LastName = "Stevens",
            Age = 30
         };
         var result = _context.Set<Contact>().Execute(x => x.InsertOne(jadaStevens));
         var indexBuilder = _context.GetPrimaryKeyProperties<Contact>();
         result.ReduceOrDefault().ShouldBe(false);
      }
   }
}