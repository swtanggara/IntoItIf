namespace IntoItIf.MongoDb.AspNetCore
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading;
   using Microsoft.Extensions.Configuration;
   using Microsoft.Extensions.Primitives;
   using MongoDb;
   using MongoDB.Bson;
   using MongoDB.Driver;

   internal class MongoDbConfigurationProvider<T> : ConfigurationProvider
      where T : class, IMongoEntity
   {
      private readonly IMongoUow _mongoUow;
      private readonly Func<T, string> _keySelector;
      private readonly Func<T, string> _elementSelector;
      private readonly Action<IMongoRepository<T>> _seedAction;
      private bool _firstLoad;

      internal MongoDbConfigurationProvider(
         IMongoUow mongoUow,
         Func<T, string> keySelector,
         Func<T, string> elementSelector,
         Action<IMongoRepository<T>> seedAction = null)
      {
         _mongoUow = mongoUow;
         _keySelector = keySelector;
         _elementSelector = elementSelector;
         _seedAction = seedAction;
         _mongoUow.RegisterChangesWatchAsync<T>(
            x =>
            {
               if (!_firstLoad && x != null) OnReload();
            },
            ChangeStreamOperationType.Insert);
         _mongoUow.RegisterChangesWatchAsync<T>(
            x =>
            {
               if (!_firstLoad && x != null) OnReload();
            },
            ChangeStreamOperationType.Update);
         _mongoUow.RegisterChangesWatchAsync<T>(
            x =>
            {
               if (!_firstLoad && x != null) OnReload();
            },
            ChangeStreamOperationType.Replace);
         _mongoUow.RegisterChangesWatchAsync<T>(
            x =>
            {
               if (!_firstLoad && x != null) OnReload();
            },
            ChangeStreamOperationType.Delete);
         ChangeToken.OnChange(
            GetReloadToken,
            () =>
            {
               Thread.Sleep(250);
               Load(false);
            });
      }


      // Load config data from EF DB.
      public override void Load()
      {
         Load(true);
      }

      private void Load(bool firstLoad)
      {
         _firstLoad = firstLoad;
         var collection = _mongoUow.SetOf<T>();
         if (_firstLoad)
         {
            var isAny = collection.LongCount() > 0;
            if (!isAny) _seedAction?.Invoke(collection);
            _firstLoad = false;
         }
         var data = collection
            .GetList(x => x, x => x.Id != default(ObjectId))
            .ToDictionary(x => _keySelector(x), x => _elementSelector(x));
         if (!IsDictionaryEqual(Data, data)) Data = data;
      }

      private static bool IsDictionaryEqual<TKey, TValue>(IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
      {
         // early-exit checks
         if (null == y)
            return null == x;
         if (null == x)
            return false;
         if (ReferenceEquals(x, y))
            return true;
         if (x.Count != y.Count)
            return false;

         // check keys are the same
         return x.Keys.All(y.ContainsKey) && x.Keys.All(k => x[k].Equals(y[k]));
      }
   }
}