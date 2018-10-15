namespace IntoItIf.Core.Domain
{
   using System;
   using Options;

   public interface IMapperProfile : IInjectable
   {
      #region Public Methods and Operators

      Option<(Type Source, Type Destination)>[] GetBinds();

      #endregion
   }
}