namespace IntoItIf.Base.Json
{
   using System;
   using System.Collections.Generic;
   using Helpers;
   using Newtonsoft.Json;
   using Newtonsoft.Json.Linq;
   using Newtonsoft.Json.Serialization;

   public class DictionaryStringPairsAsArrayOfKeyValuePairsConverter : JsonConverter<IDictionary<string, string>>
   {
      internal enum JsonNetNamingStrategy
      {
         Unspecified = 0,
         Default,
         CamelCase,
         SnakeCase,
      }

      #region Public Methods and Operators

      public override IDictionary<string, string> ReadJson(
         JsonReader reader,
         Type objectType,
         IDictionary<string, string> existingValue,
         bool hasExistingValue,
         JsonSerializer serializer)
      {
         return new Dictionary<string, string>();
      }

      public override void WriteJson(JsonWriter writer, IDictionary<string, string> value, JsonSerializer serializer)
      {
         var jToken = JToken.FromObject(value);
         if (jToken.Type == JTokenType.Object && value != null && value.Count > 0)
         {
            var namingStrategy = JsonNetNamingStrategy.Default;
            var contractResolver = serializer.ContractResolver as DefaultContractResolver;
            if (contractResolver is DefaultContractResolver defaultResolver)
            {
               switch (defaultResolver.NamingStrategy)
               {
                  case CamelCaseNamingStrategy _:
                     namingStrategy = JsonNetNamingStrategy.CamelCase;
                     break;
                  case SnakeCaseNamingStrategy _:
                     namingStrategy = JsonNetNamingStrategy.SnakeCase;
                     break;
               }
            }

            var jObject = (JObject)jToken;
            var jArray = new JArray();
            foreach (var child in jObject.Children())
            {
               var jPropChild = (JProperty)child;
               var name = jPropChild.Name;
               switch (namingStrategy)
               {
                  case JsonNetNamingStrategy.CamelCase:
                     name = StringUtils.ToCamelCase(name);
                     break;
                  case JsonNetNamingStrategy.SnakeCase:
                     name = StringUtils.ToSnakeCase(name);
                     break;
               }
               var toAddNewProp = new JProperty(name, jPropChild.Value.Value<string>());
               var toAdd = new JObject { toAddNewProp };
               jArray.Add(toAdd);
            }

            jArray.WriteTo(writer);
         }
      }

      #endregion
   }
}