namespace IntoItIf.MongoDb.AspNetCore
{
   using System;
   using Microsoft.Extensions.Configuration;
   using MongoDb;

   internal static class MongoDbExtensions
   {
      internal static IConfigurationBuilder AddMongoDbConfiguration<T>(
         this IConfigurationBuilder builder,
         IMongoUow mongoUow,
         Func<T, string> keySelector,
         Func<T, string> elementSelector,
         Action<IMongoRepository<T>> seedAction = null)
         where T : class, IMongoEntity
      {
         return builder.Add(new MongoDbConfigurationSource<T>(mongoUow, keySelector, elementSelector, seedAction));
      }
   }
}