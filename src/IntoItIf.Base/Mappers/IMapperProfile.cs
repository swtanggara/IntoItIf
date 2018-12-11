namespace IntoItIf.Base.Mappers
{
   using System;
   using Services;

   public interface IMapperProfile : IInjectable
   {
      #region Public Methods and Operators

      (Type Source, Type Destination)[] GetBinds();

      #endregion
   }
}