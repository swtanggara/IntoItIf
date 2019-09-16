namespace IntoItIf.Base.Json
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Reflection;
   using Helpers;
   using Newtonsoft.Json;
   using Newtonsoft.Json.Serialization;

   public class PropertyRenameAndIgnoreSerializerContractResolver : DefaultContractResolver
   {
      #region Fields

      private readonly Dictionary<Type, HashSet<string>> _ignores;
      private readonly Dictionary<Type, Dictionary<string, string>> _renames;

      #endregion

      #region Constructors and Destructors

      public PropertyRenameAndIgnoreSerializerContractResolver()
      {
         _ignores = new Dictionary<Type, HashSet<string>>();
         _renames = new Dictionary<Type, Dictionary<string, string>>();
      }

      #endregion

      #region Public Methods and Operators

      public PropertyRenameAndIgnoreSerializerContractResolver IgnoreProperties<T>(params Expression<Func<T, object>>[] properties)
      {
         if (properties.Length == 0) return this;
         IEnumerable<string> memberNames = properties.GetMemberNames();
         switch (NamingStrategy)
         {
            case CamelCaseNamingStrategy _:
               memberNames = memberNames.Select(StringUtils.ToCamelCase);
               break;
            case SnakeCaseNamingStrategy _:
               memberNames = memberNames.Select(StringUtils.ToSnakeCase);
               break;
         }
         InternalIgnoreProperty(typeof(T), memberNames);
         return this;
      }

      public PropertyRenameAndIgnoreSerializerContractResolver IgnoreProperties<T>(params string[] propertyNames)
      {
         if (propertyNames.Length == 0) return this;
         IEnumerable<string> memberNames = new List<string>();
         switch (NamingStrategy)
         {
            case CamelCaseNamingStrategy _:
               memberNames = propertyNames.Select(StringUtils.ToCamelCase);
               break;
            case SnakeCaseNamingStrategy _:
               memberNames = propertyNames.Select(StringUtils.ToSnakeCase);
               break;
         }
         InternalIgnoreProperty(typeof(T), memberNames);
         return this;
      }

      public PropertyRenameAndIgnoreSerializerContractResolver IgnoreProperty<T>(params string[] jsonPropertyNames)
      {
         IgnoreProperty(typeof(T), jsonPropertyNames);
         return this;
      }

      public PropertyRenameAndIgnoreSerializerContractResolver IgnoreProperty(Type type, params string[] jsonPropertyNames)
      {
         InternalIgnoreProperty(type, jsonPropertyNames);
         return this;
      }

      public PropertyRenameAndIgnoreSerializerContractResolver RenameProperty<T>(string propertyName, string newJsonPropertyName)
      {
         RenameProperty(typeof(T), propertyName, newJsonPropertyName);
         return this;
      }

      public PropertyRenameAndIgnoreSerializerContractResolver RenameProperty(Type type, string propertyName, string newJsonPropertyName)
      {
         if (!_renames.ContainsKey(type))
            _renames[type] = new Dictionary<string, string>();

         _renames[type][propertyName] = newJsonPropertyName;
         return this;
      }

      #endregion

      #region Methods

      internal PropertyRenameAndIgnoreSerializerContractResolver Copy()
      {
         var result = new PropertyRenameAndIgnoreSerializerContractResolver();
         foreach (var ignore in _ignores)
         {
            foreach (var propName in ignore.Value)
            {
               result.IgnoreProperty(ignore.Key, propName);
            }
         }

         foreach (var rename in _renames)
         {
            foreach (var oldNew in rename.Value)
            {
               result.RenameProperty(rename.Key, oldNew.Key, oldNew.Value);
            }
         }

         return result;
      }

      protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
      {
         var property = base.CreateProperty(member, memberSerialization);
         if (IsIgnored(property.DeclaringType, property.PropertyName))
         {
            property.ShouldSerialize = i => false;
            property.Ignored = true;
         }

         if (IsRenamed(property.DeclaringType, property.PropertyName, out var newJsonPropertyName))
            property.PropertyName = newJsonPropertyName;

         return property;
      }

      private void InternalIgnoreProperty(Type type, IEnumerable<string> jsonPropertyNames)
      {
         if (!_ignores.ContainsKey(type))
            _ignores[type] = new HashSet<string>();

         foreach (var prop in jsonPropertyNames)
            _ignores[type].Add(prop);
      }

      private bool IsIgnored(Type type, string jsonPropertyName)
      {
         if (!_ignores.ContainsKey(type))
            return false;

         return _ignores[type].Contains(jsonPropertyName);
      }

      private bool IsRenamed(Type type, string jsonPropertyName, out string newJsonPropertyName)
      {
         if (_renames.TryGetValue(type, out var renames) && renames.TryGetValue(jsonPropertyName, out newJsonPropertyName)) return true;
         newJsonPropertyName = null;
         return false;
      }

      #endregion
   }
}