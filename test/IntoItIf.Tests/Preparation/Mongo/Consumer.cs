namespace IntoItIf.Tests.Preparation.Mongo
{
   using System.Collections.Generic;
   using Base.Domain.Entities;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;
   using Newtonsoft.Json;

   public class Consumer : BaseEntity<Consumer>
   {
      #region Constructors and Destructors

      [JsonConstructor]
      [BsonConstructor]
      public Consumer(string code, string name, string clientId) : this(ObjectId.GenerateNewId(), code, name, clientId)
      {
      }

      public Consumer(ObjectId id, string code, string name, string clientId)
      {
         Id = id;
         Code = code;
         Name = name;
         ClientId = clientId;
      }

      #endregion

      #region Public Properties

      [BsonId]
      public ObjectId Id { get; }

      public string Code { get; }

      public string Name { get; }

      public string ClientId { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Id;
         yield return Code;
         yield return Name;
         yield return ClientId;
      }

      #endregion
   }
}