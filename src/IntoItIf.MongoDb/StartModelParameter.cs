namespace IntoItIf.MongoDb
{
   using System;
   using System.Collections.Generic;
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
         if (!BsonClassMap.IsClassMapRegistered(typeof(T))) BsonClassMap.RegisterClassMap(map);
      }
   }

}