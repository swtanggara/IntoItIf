namespace IntoItIf.MongoDb
{
   using System;
   using System.Collections.Generic;

   public sealed class MongoModelBuilder
   {
      #region Constructors and Destructors

      internal MongoModelBuilder()
      {
      }

      #endregion

      #region Properties

      internal Dictionary<Type, object> ModelDefinitions { get; set; }

      #endregion

      #region Public Methods and Operators

      public StartModelParameter<T> Entity<T>()
         where T : class
      {
         if (ModelDefinitions == null) ModelDefinitions = new Dictionary<Type, object>();
         var type = typeof(T);
         if (ModelDefinitions.ContainsKey(type)) return (StartModelParameter<T>)ModelDefinitions[type];
         ModelDefinitions[type] = new StartModelParameter<T>(this);
         return (StartModelParameter<T>)ModelDefinitions[type];
      }

      #endregion
   }
}