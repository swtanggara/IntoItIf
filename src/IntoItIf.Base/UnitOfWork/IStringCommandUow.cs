namespace IntoItIf.Base.UnitOfWork
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Domain.Options;

   public interface IStringCommandUow : IUow
   {
      #region Public Methods and Operators

      Option<int> ExecuteStringCommand(Option<string> sql, params Option<object>[] parameters);

      Task<Option<int>> ExecuteStringCommandAsync(
         Option<CancellationToken> ctok,
         Option<string> sql,
         params Option<object>[] parameters);

      Option<List<T>> FromString<T>(Option<string> sql, params Option<object>[] parameters)
         where T : class;

      Task<Option<List<T>>> FromStringAsync<T>(Option<string> sql, params Option<object>[] parameters)
         where T : class;

      Task<Option<List<T>>> FromStringAsync<T>(Option<CancellationToken> ctok, Option<string> sql, Option<object>[] parameters)
         where T : class;

      #endregion
   }
}