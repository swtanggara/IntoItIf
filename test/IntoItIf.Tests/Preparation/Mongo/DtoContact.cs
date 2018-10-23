namespace IntoItIf.Tests.Preparation.Mongo
{
   using System.Collections.Generic;
   using Base.Domain.Entities;
   using Base.Validations;
   using FluentValidation;
   using MongoDB.Bson.Serialization.Attributes;
   using MongoDB.Bson.Serialization.IdGenerators;

   public class DtoContact : BaseDto<DtoContact, DtoContactValidator>
   {
      [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
      public string Id { get; set; }

      public string FirstName { get; set; }
      public string LastName { get; set; }
      public double Age { get; set; }
      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Id;
         yield return FirstName;
         yield return LastName;
         yield return Age;
      }
   }

   public class DtoContactValidator : BaseFluentValidator<DtoContact>
   {
      public DtoContactValidator()
      {
         RuleFor(x => x.FirstName).NotEmpty();
         RuleFor(x => x.LastName).NotEmpty();
         RuleFor(x => x.Age).GreaterThan(0);
      }
   }
}