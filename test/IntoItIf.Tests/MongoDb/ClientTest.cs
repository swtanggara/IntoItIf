namespace IntoItIf.Tests.MongoDb
{
   using System;
   using System.Collections.Generic;
   using Base.Domain.Options;
   using Base.Helpers;
   using IntoItIf.MongoDb;
   using MongoDB.Driver;
   using Preparation.Mongo;
   using Shouldly;
   using Xunit;
   using Xunit.Abstractions;

   public class ClientTest
   {
      private readonly ITestOutputHelper _output;
      //private const string ConnString = "mongodb://sa:UafVaf2KMf3roLSC@localhost:27017/?authSource=admin&retryWrites=true";
      private const string ConnString = "mongodb://eq_xx:VS8vNXA5T4Kywh9H@192.168.10.7:27017/OpsigoXX?authSource=admin&authMechanism=SCRAM-SHA-256";
      //private const string ConnString =
      //   "mongodb://sa:UafVaf2KMf3roLSC@intoitifmongodb-shard-00-00-5qpey.mongodb.net:27017,intoitifmongodb-shard-00-01-5qpey.mongodb.net:27017,intoitifmongodb-shard-00-02-5qpey.mongodb.net:27017/test?ssl=true&replicaSet=IntoItIfMongoDb-shard-0&authSource=admin&retryWrites=true";
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

         List<Contact> result;
         using (var uow = new MongoUow(_context))
         {
            var optionList = uow.SetOf<Contact>().MapFlatten(x => x.GetList(y => y, y => y.FirstName == "Wildan"));
            result = optionList.ReduceOrDefault();
         }
         result.ShouldNotBeEmpty();
      }
   }
}