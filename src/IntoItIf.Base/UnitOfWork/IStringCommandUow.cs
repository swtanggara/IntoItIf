namespace IntoItIf.Base.UnitOfWork
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;

   public interface IStringCommandUow : IUow
   {
      #region Public Methods and Operators

      int ExecuteStringCommand(string sql, params object[] parameters);

      Task<int> ExecuteStringCommandAsync(
         CancellationToken ctok,
         string sql,
         params object[] parameters);

      List<T> FromString<T>(string sql, params object[] parameters)
         where T : class;

      Task<List<T>> FromStringAsync<T>(string sql, params object[] parameters)
         where T : class;

      Task<List<T>> FromStringAsync<T>(CancellationToken ctok, string sql, object[] parameters)
         where T : class;

      #endregion
   }
}