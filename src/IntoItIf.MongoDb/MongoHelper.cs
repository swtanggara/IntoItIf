namespace IntoItIf.MongoDb
{
   using System;
   using System.Linq.Expressions;
   using Base.Domain.Options;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization;
   using MongoDB.Driver;

   public static class MongoHelper
   {
      #region Methods

      public static CreateIndexModelParameter<T> Ascending<T>(
         this CreateIndexModelParameter<T> source,
         Expression<Func<T, object>> expression)
      {
         var definition = source.Keys.Ascending(expression);
         return new CreateIndexModelParameter<T>(definition, source.Options, definition.Render(), source.Builder);
      }

      public static CreateIndexModelParameter<T> Descending<T>(
         this CreateIndexModelParameter<T> source,
         Expression<Func<T, object>> expression)
      {
         var definition = source.Keys.Descending(expression);
         return new CreateIndexModelParameter<T>(definition, source.Options, definition.Render(), source.Builder);
      }

      public static CreateIndexModelParameter<T> IsUnique<T>(this CreateIndexModelParameter<T> source, bool isUnique = true)
      {
         var definition = source.Keys;
         source.Options.Unique = isUnique;
         return new CreateIndexModelParameter<T>(source.Keys, source.Options, definition.Render(), source.Builder);
      }

      private static BsonDocument Render<T>(this IndexKeysDefinition<T> definition)
      {
         var registry = BsonSerializer.SerializerRegistry;
         var serializer = registry.GetSerializer<T>();
         return definition.Render(serializer, registry);
      }

      internal static Option<FindOptions> GetFindOptions(this Option<MongoDataContext> mongoContext)
      {
         return mongoContext.MapFlatten(x => x.FindOptions);
      }

      #endregion
   }
}