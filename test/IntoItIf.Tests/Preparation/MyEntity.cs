namespace IntoItIf.Tests.Preparation
{
   using System.Collections.Generic;
   using Base.Domain.Entities;
   using Base.Domain.Options;
   using Base.Mappers;
   using Base.Validations;

   public class MyEntity : BaseEntity<MyEntity>
   {
      #region Constructors and Destructors

      public MyEntity(Option<int> id, Option<string> name)
      {
         Id = id.ReduceOrDefault();
         Name = name.ReduceOrDefault();
      }

      public MyEntity(IDataValidator<MyEntity> validator) : base(validator)
      {
      }

      public MyEntity(IMapperService mapperService, IDataValidator<MyEntity> validator) : base(
         mapperService,
         validator)
      {
      }

      private MyEntity()
      {
      }

      #endregion

      #region Public Properties

      public int Id { get; set; }
      public string Name { get; set; }

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