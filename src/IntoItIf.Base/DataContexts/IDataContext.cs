namespace IntoItIf.Base.DataContexts
{
   using System;
   using System.Linq;
   using Domain.Options;
   using Services;

   public interface IDataContext : IDisposable, IInjectable
   {
      #region Public Methods and Operators

      Option<IQueryable<T>> GetQuery<T>()
         where T : class;

      #endregion
   }
}