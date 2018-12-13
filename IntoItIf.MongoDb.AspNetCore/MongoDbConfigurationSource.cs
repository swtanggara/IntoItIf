namespace IntoItIf.MongoDb.AspNetCore
{
   using System;
   using Microsoft.Extensions.Configuration;
   using MongoDb;

   internal class MongoDbConfigurationSource<T> : IConfigurationSource
      where T : class, IMongoEntity
   {
      private readonly IMongoUow _mongoUow;
      private readonly Func<T, string> _keySelector;
      private readonly Func<T, string> _elementSelector;
      private readonly Action<IMongoRepository<T>> _seedAction;

      internal MongoDbConfigurationSource(
         IMongoUow mongoUow,
         Func<T, string> keySelector,
         Func<T, string> elementSelector,
         Action<IMongoRepository<T>> seedAction = null)
      {
         _mongoUow = mongoUow;
         _keySelector = keySelector;
         _elementSelector = elementSelector;
         _seedAction = seedAction;
      }

      public IConfigurationProvider Build(IConfigurationBuilder builder)
      {
         return new MongoDbConfigurationProvider<T>(_mongoUow, _keySelector, _elementSelector, _seedAction);
      }
   }
}