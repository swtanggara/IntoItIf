namespace IntoItIf.MongoDb
{
   using System;
   using MongoDB.Bson;
   using Newtonsoft.Json;

   public class StringObjectIdJsonConverter : JsonConverter<ObjectId>
   {
      public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
      {
         writer.WriteValue(value.ToString());
      }

      public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
      {
         var value = reader.Value;
         var tokenType = reader.TokenType;
         if (tokenType == JsonToken.String) return ObjectId.Parse((string)value);
         throw new NotSupportedException($"Cannot convert from {tokenType} to ObjectId");
      }
   }
}