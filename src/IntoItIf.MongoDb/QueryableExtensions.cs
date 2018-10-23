namespace IntoItIf.MongoDb
{
   using System.Linq;
   using Base.Domain.Options;
   using MongoDB.Driver.Linq;

   internal static class QueryableExtensions
   {
      #region Methods

      internal static IMongoQueryable<T> ToMongoQueryable<T>(this IQueryable<T> source)
      {
         return source as IMongoQueryable<T>;
      }

      internal static Option<IMongoQueryable<T>> ToOptionMongoQueryable<T>(this Option<IQueryable<T>> queryable)
      {
         return queryable.Map(x => x.ToMongoQueryable());
      }

      internal static Option<IQueryable<T>> ToOptionQueryable<T>(this Option<IMongoQueryable<T>> mongoQueryable)
      {
         return mongoQueryable.Map(x => x.ToQueryable());
      }

      internal static IQueryable<T> ToQueryable<T>(this IMongoQueryable<T> source)
      {
         return source;
      }

      #endregion
   }
}