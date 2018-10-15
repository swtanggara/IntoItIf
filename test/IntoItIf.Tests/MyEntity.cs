namespace IntoItIf.Tests
{
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;

   public class MyEntity : BaseEntity<MyEntity>
   {
      #region Constructors and Destructors

      public MyEntity(Option<IDataValidator<MyEntity>> validator) : base(validator)
      {
      }

      public MyEntity(Option<IMapperService> mapperService, Option<IDataValidator<MyEntity>> validator) : base(
         mapperService,
         validator)
      {
      }

      #endregion
   }
}