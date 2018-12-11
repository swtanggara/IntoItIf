namespace IntoItIf.Base.DataContexts
{
   using System;
   using System.Linq;
   using Services;

   public interface IDataContext : IDisposable, IInjectable
   {
      #region Public Methods and Operators

      IQueryable<T> GetQuery<T>()
         where T : class;

      #endregion
   }
}