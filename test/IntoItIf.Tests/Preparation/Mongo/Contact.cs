namespace IntoItIf.Tests.Preparation.Mongo
{
   using System;
   using System.Collections.Generic;
   using Base.Domain.Entities;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;

   public class Contact : BaseEntity<Contact, ContactValidator>
   {
      #region Constructors and Destructors

      public Contact(string firstName, string lastName, double age) : this(ObjectId.GenerateNewId(), firstName, lastName, age)
      {
      }

      public Contact(ObjectId id, string firstName, string lastName, double age)
      {
         Id = id;
         FirstName = firstName;
         LastName = lastName;
         Age = age;
      }

      #endregion

      #region Public Properties

      [BsonId]
      public ObjectId Id { get; }

      public string FirstName { get; }
      public string LastName { get; }
      public double Age { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Id;
         yield return FirstName;
         yield return LastName;
         yield return Age;
      }

      #endregion
   }
}