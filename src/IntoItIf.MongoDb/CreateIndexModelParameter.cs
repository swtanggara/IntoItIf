namespace IntoItIf.MongoDb
{
   using System;
   using MongoDB.Bson;
   using MongoDB.Driver;

   public abstract class CreateIndexModelParameter
   {
      #region Constructors and Destructors

      protected internal CreateIndexModelParameter(Type type, CreateIndexOptions options, BsonDocument rendered, MongoModelBuilder builder)
      {
         Type = type;
         Options = options;
         Rendered = rendered;
         Builder = builder;
         SetIndexesOnBuilder(Builder);
      }

      #endregion

      #region Properties

      internal MongoModelBuilder Builder { get; }
      internal CreateIndexOptions Options { get; }
      internal BsonDocument Rendered { get; }
      internal Type Type { get; }

      #endregion

      #region Methods

      private void SetIndexesOnBuilder(MongoModelBuilder builder)
      {
         if (builder.ModelDefinitions.ContainsKey(Type) &&
             builder.ModelDefinitions[Type] is CreateIndexModelParameter parameter &&
             parameter.Type == Type)
         {
            builder.ModelDefinitions[Type] = this;
         }
      }

      #endregion
   }

   public sealed class CreateIndexModelParameter<T> : CreateIndexModelParameter
   {
      #region Constructors and Destructors

      internal CreateIndexModelParameter(
         IndexKeysDefinition<T> keys,
         CreateIndexOptions options,
         BsonDocument rendered,
         MongoModelBuilder builder) : base(typeof(T), options, rendered, builder)
      {
         Keys = keys;
      }

      #endregion

      #region Properties

      internal IndexKeysDefinition<T> Keys { get; }

      #endregion
   }
}