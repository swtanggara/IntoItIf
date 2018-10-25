namespace IntoItIf.MongoDb
{
   using System;
   using System.Collections.Generic;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization;
   using MongoDB.Driver;

   public abstract class StartModelParameter
   {
      protected StartModelParameter(Type type, MongoModelBuilder modelBuilder)
      {
         Type = type;
         ModelBuilder = modelBuilder;
         IndexModelParameters = new List<CreateIndexModelParameter>();
      }

      internal Type Type { get; }

      internal List<CreateIndexModelParameter> IndexModelParameters { get; }
      protected MongoModelBuilder ModelBuilder { get; }
   }

   public class StartModelParameter<T> : StartModelParameter
   {
      public StartModelParameter(MongoModelBuilder modelBuilder) : base(typeof(T), modelBuilder)
      {
      }

      public void SetIndexes(params Func<CreateIndexModelParameter<T>, CreateIndexModelParameter<T>>[] indexCreations)
      {
         foreach (var indexCreation in indexCreations)
         {
            var builder = Builders<T>.IndexKeys;
            var keys = builder.Combine();
            var options = new CreateIndexOptions();
            var startParameter = new CreateIndexModelParameter<T>(keys, options, null, ModelBuilder);
            var resultParameter = indexCreation.Invoke(startParameter);
            IndexModelParameters.Add(resultParameter);
         }
      }

      public void CreateMap(Action<BsonClassMap<T>> map)
      {
         BsonClassMap.RegisterClassMap(map);
      }
   }

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