namespace IntoItIf.Base.Json
{
   using System;
   using Domain;
   using Newtonsoft.Json;

   public class EnumerationJsonConverter<T> : JsonConverter<T>
      where T : Enumeration<T>
   {
      public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
      {
         writer.WriteValue(value.Id);
      }

      public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
      {
         var value = reader.Value;
         var tokenType = reader.TokenType;
         switch (tokenType)
         {
            case JsonToken.Integer:
            {
               var longValue = (long)value;
               return Enumeration<T>.FromValue((int)longValue);
            }
            case JsonToken.String:
               return Enumeration<T>.Parse((string)value);
            case JsonToken.Null:
               return Enumeration<T>.FromValue(-1);
            default:
               throw new NotSupportedException($"Cannot convert from {tokenType} to {typeof(T).Name}");
         }
      }
   }
}