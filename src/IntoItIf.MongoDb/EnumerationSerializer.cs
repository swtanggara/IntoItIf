namespace IntoItIf.MongoDb
{
   using System;
   using Base.Domain;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization;
   using MongoDB.Bson.Serialization.Serializers;

   public class EnumerationSerializer<T> : SerializerBase<T>
      where T : Enumeration<T>
   {
      public override T Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
      {
         var type = context.Reader.GetCurrentBsonType();
         switch (type)
         {
            case BsonType.Int32:
               return Enumeration<T>.FromValue(context.Reader.ReadInt32());
            case BsonType.String:
               return Enumeration<T>.Parse(context.Reader.ReadString());
            default:
               throw new NotSupportedException($"Cannot convert from {type} to {typeof(T).Name}");
         }
      }

      public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
      {
         context.Writer.WriteInt32(value.Id);
      }
   }
}