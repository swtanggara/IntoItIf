namespace IntoItIf.Tests.Preparation.Preparation.Mongo
{
   using System;
   using Base.Domain;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;
   using MongoDB.Bson.Serialization.IdGenerators;

   public class People
   {
      #region Public Properties

      [BsonId(IdGenerator = typeof(GuidGenerator))]
      public Guid Id { get; set; }

      public string IdNumber { get; set; }
      public string NickName { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }

      public BsonDateTime BirthDate { get; set; }

      public int HairColor { get; set; }

      #endregion
   }

   public class HairColor : Enumeration<HairColor>
   {
      #region Static Fields

      public static readonly HairColor Blonde = new HairColor(1, nameof(Blonde));

      public static readonly HairColor Brunette = new HairColor(0, nameof(Brunette));
      public static readonly HairColor RedHead = new HairColor(2, nameof(RedHead));

      #endregion

      #region Constructors and Destructors

      public HairColor(int id, string name) : base(id, name)
      {
      }

      #endregion
   }
}