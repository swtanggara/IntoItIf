namespace IntoItIf.Base.Mappers
{
   using System;
   using Domain.Options;
   using Services;

   public interface IMapperProfile : IInjectable
   {
      #region Public Methods and Operators

      Option<(Type Source, Type Destination)>[] GetBinds();

      #endregion
   }
}