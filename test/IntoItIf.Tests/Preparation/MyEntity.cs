namespace IntoItIf.Tests.Preparation
{
   using System.Collections.Generic;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;

   public class MyEntity : BaseEntity<MyEntity>
   {
      #region Constructors and Destructors

      public MyEntity(Option<int> id, Option<string> name)
      {
         Id = id.ReduceOrDefault();
         Name = name.ReduceOrDefault();
      }

      public MyEntity(Option<IDataValidator<MyEntity>> validator) : base(validator)
      {
      }

      public MyEntity(Option<IMapperService> mapperService, Option<IDataValidator<MyEntity>> validator) : base(
         mapperService,
         validator)
      {
      }

      private MyEntity()
      {
      }

      #endregion

      #region Public Properties

      public int Id { get; }
      public string Name { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Id;
         yield return Name;
      }

      #endregion
   }
}