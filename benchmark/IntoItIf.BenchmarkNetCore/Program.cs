using System;

namespace IntoItIf.BenchmarkNetCore
{
   using MongoDb;
   using Tests.Preparation.Preparation.Mongo;

   class Program
   {
      static void Main(string[] args)
      {
         var uow = new MongoUow(new MyMongoDataContext());
      }
   }
}
