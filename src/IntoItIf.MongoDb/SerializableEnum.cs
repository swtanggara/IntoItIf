namespace IntoItIf.MongoDb
{
   using Base.Domain;

   public abstract class SerializableEnum<T> : Enumeration<T>
      where T : Enumeration<T>
   {
      protected SerializableEnum(int id, string name) : base(id, name)
      {
      }

      public static EnumerationSerializer<T> GetSerializer()
      {
         return new EnumerationSerializer<T>();
      }
   }
}