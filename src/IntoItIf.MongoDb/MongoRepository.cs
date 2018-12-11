namespace IntoItIf.MongoDb
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain;
   using Base.Helpers;
   using Base.Repositories;
   using MongoDB.Driver;

   public sealed class MongoRepository<T> : BaseRepository<T>, IMongoRepository<T>
      where T : class
   {
      #region Constructors and Destructors

      public MongoRepository(MongoDataContext dataContext) : this(dataContext, null)
      {
      }

      public MongoRepository(MongoDataContext dataContext, IQueryable<T> customQuery) : base(dataContext, customQuery)
      {
         MongoDataContext = dataContext;
      }

      #endregion

      #region Properties

      private MongoDataContext MongoDataContext { get; }

      #endregion

      #region Public Methods and Operators

      public override Dictionary<string, object> Add(T entity, Func<T, string> existMessageFunc)
      {
         return Add(entity, existMessageFunc, null);
      }

      public Dictionary<string, object> Add(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session)
      {
         return ProcessCreateAndGetResult(GetValidatedEntityForCreate(entity, session), existMessageFunc, session);
      }

      public override Task<Dictionary<string, object>> AddAsync(
         T entity,
         Func<T, string> existMessageFunc,
         CancellationToken ctok)
      {
         return AddAsync(entity, existMessageFunc, null, ctok);
      }

      public Task<Dictionary<string, object>> AddAsync(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session)
      {
         return AddAsync(entity, existMessageFunc, session, CancellationToken.None);
      }

      public Task<Dictionary<string, object>> AddAsync(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         return ProcessCreateAndGetResultAsync(GetValidatedEntityForCreateAsync(entity, session, ctok), existMessageFunc, session, ctok);
      }

      public Dictionary<string, object> Change(T entity, params Expression<Func<T, object>>[] fieldSelections)
      {
         return Change(entity, null, null, fieldSelections);
      }

      public Dictionary<string, object> Change(
         T entity,
         Func<T, string> existMessageFunc,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         return Change(entity, existMessageFunc, null, fieldSelections);
      }

      public Dictionary<string, object> Change(
         T entity,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         return Change(entity, null, null, fieldSelections);
      }

      public override Dictionary<string, object> Change(T entity, Func<T, string> existMessageFunc)
      {
         return Change(entity, existMessageFunc, session: null, null);
      }

      public Dictionary<string, object> Change(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         return ProcessUpdateAndGetResult(GetValidatedEntityForUpdate(entity, session), existMessageFunc, session, fieldSelections);
      }

      public Task<Dictionary<string, object>> ChangeAsync(T entity, params Expression<Func<T, object>>[] fieldSelections)
      {
         return ChangeAsync(entity, null, null, CancellationToken.None, fieldSelections);
      }

      public Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         return ChangeAsync(entity, null, session, CancellationToken.None, fieldSelections);
      }

      public override Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         CancellationToken ctok)
      {
         return ChangeAsync(entity, existMessageFunc, null, ctok);
      }

      public Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         return ChangeAsync(entity, existMessageFunc, session, ctok, null);
      }

      public Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         CancellationToken ctok,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         return ChangeAsync(entity, null, null, ctok, fieldSelections);
      }

      public Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         return ChangeAsync(entity, existMessageFunc, null, CancellationToken.None, fieldSelections);
      }

      public Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         IClientSessionHandle session,
         CancellationToken ctok,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         return ChangeAsync(entity, null, session, ctok, fieldSelections);
      }

      public Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         CancellationToken ctok,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         return ChangeAsync(entity, existMessageFunc, null, ctok, fieldSelections);
      }

      public Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         return ChangeAsync(entity, existMessageFunc, session, CancellationToken.None, fieldSelections);
      }

      public Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         CancellationToken ctok,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         return ProcessUpdateAndGetResultAsync(
            GetValidatedEntityForUpdateAsync(entity, session, ctok),
            existMessageFunc,
            session,
            ctok,
            fieldSelections);
      }

      public override T GetFirstOrDefault(Expression<Func<T, bool>> predicate)
      {
         return GetFirstOrDefault(x => x, predicate);
      }

      public T GetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort)
      {
         return GetFirstOrDefault(x => x, predicate, sort);
      }

      public T GetFirstOrDefault(Expression<Func<T, bool>> predicate, IClientSessionHandle session)
      {
         return GetFirstOrDefault(predicate, sort: null, session);
      }

      public T GetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session)
      {
         return GetFirstOrDefault(x => x, predicate, sort, session);
      }

      public override TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate)
      {
         return GetFirstOrDefault(selector, predicate, null, null);
      }

      public TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort)
      {
         return GetFirstOrDefault(selector, predicate, sort, null);
      }

      public TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session)
      {
         return GetFirstOrDefault(selector, predicate, null, session);
      }

      public TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session)
      {
         return InternalGetFirstOrDefault(selector, predicate, sort, session);
      }

      public override Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(x => x, predicate, ctok);
      }

      public Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session)
      {
         return GetFirstOrDefaultAsync(predicate, null, session);
      }

      public Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort)
      {
         return GetFirstOrDefaultAsync(predicate, sort, null);
      }

      public Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session)
      {
         return GetFirstOrDefaultAsync(predicate, sort, session, CancellationToken.None);
      }

      public Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(predicate, sort: null, session, ctok);
      }

      public Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(x => x, predicate, sort, session, ctok);
      }

      public override Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, sort:null, ctok);
      }

      public Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort)
      {
         return GetFirstOrDefaultAsync(selector, predicate, sort, null,CancellationToken.None);
      }

      public Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, sort, null, ctok);
      }

      public Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, null, session, ctok);
      }

      public Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         return InternalGetFirstOrDefaultAsync(selector, predicate, sort, session, ctok);
      }

      public override List<TResult> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate)
      {
         return GetList(selector, predicate, null);
      }

      public List<TResult> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy)
      {
         return GetList(selector, predicate, sortBy, null);
      }

      public List<TResult> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy,
         IClientSessionHandle session)
      {
         return InternalGetList(selector, predicate, sortBy, session);
      }

      public override Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
      {
         return GetListAsync(selector, predicate, null, ctok);
      }

      public Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy,
         CancellationToken ctok)
      {
         return GetListAsync(selector, predicate, sortBy, null, ctok);
      }

      public Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         return InternalGetListAsync(selector, predicate, sortBy, session, ctok);
      }

      public override List<KeyValue> GetLookups(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate)
      {
         return GetLookups(idProperty, valueProperty, useValueAsId, predicate, null);
      }

      public List<KeyValue> GetLookups(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session)
      {
         return InternalGetLookups(idProperty, valueProperty, useValueAsId, predicate, session);
      }

      public override Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, predicate, null, ctok);
      }

      public Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         return InternalGetLookupsAsync(idProperty, valueProperty, useValueAsId, predicate, session, ctok);
      }

      public override IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate)
      {
         return GetPaged(x => x, searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate);
      }

      public override IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate)
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, null);
      }

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session)
      {
         return InternalGetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, session);
      }

      public override Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
      {
         return GetPagedAsync(
            x => x,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            predicate,
            ctok);
      }

      public override Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
      {
         return GetPagedAsync(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            predicate,
            null,
            ctok);
      }

      public Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         return InternalGetPagedAsync(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            predicate,
            session,
            ctok);
      }

      public override long LongCount()
      {
         return LongCount(null);
      }

      public long LongCount(Expression<Func<T, bool>> predicate)
      {
         return LongCount(predicate, null);
      }

      public long LongCount(Expression<Func<T, bool>> predicate, IClientSessionHandle session)
      {
         return InternalLongCount(predicate, session);
      }

      public override Task<long> LongCountAsync(CancellationToken ctok)
      {
         return LongCountAsync(null, ctok);
      }

      public Task<long> LongCountAsync(Expression<Func<T, bool>> predicate)
      {
         return LongCountAsync(predicate, CancellationToken.None);
      }

      public Task<long> LongCountAsync(Expression<Func<T, bool>> predicate, CancellationToken ctok)
      {
         return LongCountAsync(predicate, null, ctok);
      }

      public Task<long> LongCountAsync(
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         return InternalLongCountAsync(predicate, session, ctok);
      }

      public override bool Remove(T entity)
      {
         return Remove(entity, null);
      }

      public bool Remove(T entity, IClientSessionHandle session)
      {
         return ProcessDeleteAndGetResult(GetValidatedEntityForDelete(entity, session), session);
      }

      public override Task<bool> RemoveAsync(T entity, CancellationToken ctok)
      {
         return RemoveAsync(entity, null, ctok);
      }

      public Task<bool> RemoveAsync(T entity, IClientSessionHandle session, CancellationToken ctok)
      {
         return ProcessDeleteAndGetResultAsync(GetValidatedEntityForDeleteAsync(entity, session, ctok), session, ctok);
      }

      #endregion

      #region Methods

      /// <summary>
      ///    Not Implemented
      /// </summary>
      /// <param name="sourceQuery"></param>
      /// <param name="predicate"></param>
      /// <returns></returns>
      protected override IQueryable<T> BuildQuery(IQueryable<T> sourceQuery, Expression<Func<T, bool>> predicate)
      {
         throw new NotImplementedException();
      }

      private static IFindFluent<T, TResult> GetPerPageFindFluent<TResult>(
         IMongoCollection<T> set,
         FindOptions findOptions,
         Expression<Func<T, TResult>> selector,
         IPageQuery pageQuery,
         string[] defaultSortKeys,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session)
      {
         var keywordPredicate = SearchInPropertyNames<T>.BuildPredicate(pageQuery.Keyword, pageQuery.SearchFields.Pascalize());
         var exprBody = Expression.AndAlso(predicate, keywordPredicate);
         var inPredicate = Expression.Lambda<Func<T, bool>>(exprBody, predicate.Parameters[0]);
         var findFluent = session == null
            ? set.Find(inPredicate, findOptions)
            : set.Find(session, inPredicate, findOptions);
         var pageSize = PageQuery.DefaultPageSize;
         var pageIndex = PageQuery.DefaultIndexFrom;
         if (pageQuery.PageSize > 0) pageSize = pageQuery.PageSize;
         if (pageQuery.PageIndex > 0) pageIndex = pageQuery.PageIndex;
         var sortDefinition = Builders<T>.Sort.Combine();
         if (pageQuery.Sorts != null && PagedIQueryable.IsValidSorts<T>(pageQuery.Sorts))
         {
            foreach (var sort in pageQuery.Sorts)
            {
               if (sort.StartsWith("+"))
                  sortDefinition = sortDefinition.Ascending(new StringFieldDefinition<T>(sort.TrimStart('+')));
               else if (sort.StartsWith("-"))
                  sortDefinition = sortDefinition.Descending(new StringFieldDefinition<T>(sort.TrimStart('-')));
               else
                  sortDefinition = sortDefinition.Ascending(new StringFieldDefinition<T>(sort));
            }
         }
         else
         {
            sortDefinition = defaultSortKeys.Aggregate(
               sortDefinition,
               (current, defaultSort) => current.Ascending(new StringFieldDefinition<T>(defaultSort)));
         }

         return findFluent.Sort(sortDefinition)
            .Skip((pageIndex - pageQuery.IndexFrom.Id) * pageSize)
            .Limit(pageSize)
            .Project(selector);
      }

      private static UpdateDefinition<T> GetSelectedUpdateDefinition(T entity, IReadOnlyList<Expression<Func<T, object>>> fieldSelections)
      {
         if (fieldSelections == null || !fieldSelections.Any()) return null;
         var first = fieldSelections.First();
         var updateDefinition = Builders<T>.Update.Set(first, first.Compile().Invoke(entity));
         for (var i = 1; i <= fieldSelections.Count - 1; i++)
         {
            var selection = fieldSelections[i];
            updateDefinition = updateDefinition.Set(selection, selection.Compile().Invoke(entity));
         }

         return updateDefinition;
      }

      private Dictionary<string, object> CreateAndGetKeyValues(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity, Func<T, string> MessageFunc) validated,
         IClientSessionHandle session)
      {
         var createSuccess = CreateEntry(validated.InputEntity, session);
         return createSuccess ? RepositoryHelper<T>.GetKeysAndValues(validated.PropertyNames, validated.InputEntity) : null;
      }

      private async Task<Dictionary<string, object>> CreateAndGetKeyValuesAsync(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity, Func<T, string> MessageFunc) validated,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         var createSuccess = await CreateEntryAsync(validated.InputEntity, session, ctok);
         return createSuccess ? RepositoryHelper<T>.GetKeysAndValues(validated.PropertyNames, validated.InputEntity) : null;
      }

      private bool CreateEntry(T entry, IClientSessionHandle session)
      {
         var set = MongoDataContext.Collection<T>();
         if (session == null)
         {
            set.InsertOne(entry);
            return true;
         }
         set.InsertOne(session, entry);
         return true;
      }

      private async Task<bool> CreateEntryAsync(
         T entry,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         var set = MongoDataContext.Collection<T>();
         if (session == null)
         {
            await set.InsertOneAsync(entry, cancellationToken:ctok);
            return true;
         }
         await set.InsertOneAsync(session, entry, cancellationToken: ctok);
         return true;
      }

      private bool DeleteEntry(T exist, IClientSessionHandle session)
      {
         var pkPredicate = MongoDataContext.BuildPrimaryKeyPredicate(exist);
         var result = session == null
            ? MongoDataContext.Collection<T>().DeleteOne(pkPredicate.Predicate)
            : MongoDataContext.Collection<T>().DeleteOne(session, pkPredicate.Predicate);
         return result.IsAcknowledged && result.DeletedCount > 0;
      }

      private async Task<bool> DeleteEntryAsync(
         T exist,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         var pkPredicate = MongoDataContext.BuildPrimaryKeyPredicate(exist);
         var result = session == null
            ? await MongoDataContext.Collection<T>().DeleteOneAsync(pkPredicate.Predicate, ctok)
            : await MongoDataContext.Collection<T>().DeleteOneAsync(session, pkPredicate.Predicate, cancellationToken: ctok);
         return result.IsAcknowledged && result.DeletedCount > 0;
      }

      private (T MatchValidatedEntity, string[] PropertyNames, T InputEntity) GetValidatedEntityForCreate(
         T entity, 
         IClientSessionHandle session)
      {
         var pkPredicate = MongoDataContext.BuildPrimaryKeyPredicate(entity);
         var pkExist = InternalGetFirstOrDefault(pkPredicate.Predicate, null, session);
         if (pkExist != null) return (pkExist, pkPredicate.PropertyNames, entity);
         var akPredicate = MongoDataContext.BuildAlternateKeyPredicate(entity);
         var akExist = InternalGetFirstOrDefault(akPredicate.Predicate, null, session);
         return akExist != null ? (akExist, akPredicate.PropertyNames, entity) : (null, akPredicate.PropertyNames, entity);
      }

      private async Task<(T MatchValidatedEntity, string[] PropertyNames, T InputEntity)> GetValidatedEntityForCreateAsync(
         T entity,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         var pkPredicate = MongoDataContext.BuildPrimaryKeyPredicate(entity);
         var pkExist = await InternalGetFirstOrDefaultAsync(pkPredicate.Predicate, null, session, ctok);
         if (pkExist != null) return (pkExist, pkPredicate.PropertyNames, entity);
         var akPredicate = MongoDataContext.BuildAlternateKeyPredicate(entity);
         var akExist = await InternalGetFirstOrDefaultAsync(akPredicate.Predicate, null, session, ctok);
         return akExist != null ? (akExist, akPredicate.PropertyNames, entity) : (null, akPredicate.PropertyNames, entity);
      }

      private (T Entity, T Exist) GetValidatedEntityForDelete(
         T entity,
         IClientSessionHandle session)
      {
         var predicate = entity.BuildPredicate();
         var exist = InternalGetFirstOrDefault(predicate, null, session);
         return (entity, exist);
      }

      private async Task<(T Entity, T Exist)> GetValidatedEntityForDeleteAsync(
         T entity,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         var predicate = entity.BuildPredicate();
         var exist = await InternalGetFirstOrDefaultAsync(predicate, null, session, ctok);
         return (entity, exist);
      }

      private (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity) GetValidatedEntityForUpdate(
         T entity, 
         IClientSessionHandle session)
      {
         var pkPredicate = MongoDataContext.BuildPrimaryKeyPredicate(entity);
         var akPredicate = MongoDataContext.BuildAlternateKeyPredicate(entity);
         var keyProperties =
            RelationalRepositoryHelper<T>.GetKeyPropertyNamesInBetween(pkPredicate.PropertyNames, akPredicate.PropertyNames);
         var mapped = (
            ExistByPkEntity: InternalGetFirstOrDefault(pkPredicate.Predicate, null, session),
            ExistByAkEntity: InternalGetFirstOrDefault(akPredicate.Predicate, null, session),
            RealKeyPropertyNames: keyProperties,
            PkPropertyNames: pkPredicate.PropertyNames,
            InputEntity: entity);
         return RelationalRepositoryHelper<T>.GiveValidatedEntityForUpdateResult(mapped);
      }

      private async Task<(T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity)> GetValidatedEntityForUpdateAsync(
         T entity, 
         IClientSessionHandle session, 
         CancellationToken ctok)
      {
         var pkPredicate = MongoDataContext.BuildPrimaryKeyPredicate(entity);
         var akPredicate = MongoDataContext.BuildAlternateKeyPredicate(entity);
         var keyProperties =
            RelationalRepositoryHelper<T>.GetKeyPropertyNamesInBetween(pkPredicate.PropertyNames, akPredicate.PropertyNames);
         var mapped = (
            ExistByPkEntity: await InternalGetFirstOrDefaultAsync(pkPredicate.Predicate, null, session, ctok),
            ExistByAkEntity: await InternalGetFirstOrDefaultAsync(akPredicate.Predicate, null, session, ctok),
            RealKeyPropertyNames: keyProperties,
            PkPropertyNames: pkPredicate.PropertyNames,
            InputEntity: entity);
         return RelationalRepositoryHelper<T>.GiveValidatedEntityForUpdateResult(mapped);
      }

      private T InternalGetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session)
      {
         return InternalGetFirstOrDefault(x => x, predicate, sort, session);
      }

      private TResult InternalGetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session)
      {
         var set = MongoDataContext.Collection<T>();
         var realSort = sort.GetValueOrDefault();
         if (session == null)
         {
            if (!sort.HasValue) return set.Find(predicate).Project(selector).FirstOrDefault();
            if (realSort.Ascending)
            {
               return set.Find(predicate).SortBy(realSort.SortBy).Project(selector).FirstOrDefault();
            }

            return realSort.Ascending
               ? set.Find(predicate).SortBy(realSort.SortBy).Project(selector).FirstOrDefault()
               : set.Find(predicate).SortByDescending(realSort.SortBy).Project(selector).FirstOrDefault();
         }

         if (!sort.HasValue) return set.Find(session, predicate).Project(selector).FirstOrDefault();
         if (realSort.Ascending)
         {
            return set.Find(session, predicate).SortBy(realSort.SortBy).Project(selector).FirstOrDefault();
         }

         return realSort.Ascending
            ? set.Find(session, predicate).SortBy(realSort.SortBy).Project(selector).FirstOrDefault()
            : set.Find(session, predicate).SortByDescending(realSort.SortBy).Project(selector).FirstOrDefault();
      }

      private Task<T> InternalGetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         return InternalGetFirstOrDefaultAsync(x => x, predicate, sort, session, ctok);
      }

      private Task<TResult> InternalGetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         var set = MongoDataContext.Collection<T>();
         var realSort = sort.GetValueOrDefault();
         if (session == null)
         {
            if (!sort.HasValue) return set.Find(predicate).Project(selector).FirstOrDefaultAsync(ctok);
            if (realSort.Ascending)
            {
               return set.Find(predicate).SortBy(realSort.SortBy).Project(selector).FirstOrDefaultAsync(ctok);
            }

            return realSort.Ascending
               ? set.Find(predicate).SortBy(realSort.SortBy).Project(selector).FirstOrDefaultAsync(ctok)
               : set.Find(predicate).SortByDescending(realSort.SortBy).Project(selector).FirstOrDefaultAsync(ctok);
         }

         if (!sort.HasValue) return set.Find(session, predicate).Project(selector).FirstOrDefaultAsync(ctok);
         if (realSort.Ascending)
         {
            return set.Find(session, predicate).SortBy(realSort.SortBy).Project(selector).FirstOrDefaultAsync(ctok);
         }

         return realSort.Ascending
            ? set.Find(session, predicate).SortBy(realSort.SortBy).Project(selector).FirstOrDefaultAsync(ctok)
            : set.Find(session, predicate).SortByDescending(realSort.SortBy).Project(selector).FirstOrDefaultAsync(ctok);
      }

      private List<TResult> InternalGetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy,
         IClientSessionHandle session)
      {
         var set = MongoDataContext.Collection<T>();
         var findOptions = MongoDataContext.FindOptions;
         if (session == null)
         {
            return sortBy == null
               ? set.Find(predicate, findOptions).Project(selector).ToList()
               : sortBy(set.Find(predicate, findOptions)).Project(selector).ToList();
         }

         return sortBy == null
            ? set.Find(session, predicate, findOptions).Project(selector).ToList()
            : sortBy(set.Find(session, predicate, findOptions)).Project(selector).ToList();
      }

      private Task<List<TResult>> InternalGetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         var set = MongoDataContext.Collection<T>();
         var findOptions = MongoDataContext.FindOptions;
         if (session == null)
         {
            return sortBy == null
               ? set.Find(predicate, findOptions).Project(selector).ToListAsync(ctok)
               : sortBy(set.Find(predicate, findOptions)).Project(selector).ToListAsync(ctok);
         }

         return sortBy == null
            ? set.Find(session, predicate, findOptions).Project(selector).ToListAsync(ctok)
            : sortBy(set.Find(session, predicate, findOptions)).Project(selector).ToListAsync(ctok);
      }

      private List<KeyValue> InternalGetLookups(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session)
      {
         var set = MongoDataContext.Collection<T>();
         var findOptions = MongoDataContext.FindOptions;
         var orderByField = useValueAsId ? valueProperty : idProperty;
         var findFluent = session == null
            ? set.Find(predicate, findOptions)
            : set.Find(session, predicate, findOptions);
         return findFluent
            .Sort(Builders<T>.Sort.Ascending(orderByField))
            .Project(y => RepositoryHelper<T>.ToKeyValue(y, idProperty, valueProperty, useValueAsId))
            .ToList();
      }

      private Task<List<KeyValue>> InternalGetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         var set = MongoDataContext.Collection<T>();
         var findOptions = MongoDataContext.FindOptions;
         var orderByField = useValueAsId ? valueProperty : idProperty;
         var findFluent = session == null
            ? set.Find(predicate, findOptions)
            : set.Find(session, predicate, findOptions);
         return findFluent
            .Sort(Builders<T>.Sort.Ascending(orderByField))
            .Project(y => RepositoryHelper<T>.ToKeyValue(y, idProperty, valueProperty, useValueAsId))
            .ToListAsync(ctok);
      }

      private IPaged<TResult> InternalGetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session)
      {
         var pageQuery = PageQuery.Get(pageIndex, pageSize, sorts, keyword, indexFrom, searchFields);
         var defaultSortKeys = RelationalRepositoryHelper<T>.GetSortKeys(MongoDataContext);
         var findFluent = GetPerPageFindFluent(
               MongoDataContext.Collection<T>(),
               MongoDataContext.FindOptions,
               selector,
               pageQuery,
               defaultSortKeys,
               predicate,
               session);
         return new Paged<TResult>(findFluent.ToList(), findFluent.CountDocuments(), pageIndex, pageSize, pageQuery.IndexFrom);
      }

      private async Task<IPaged<TResult>> InternalGetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         var pageQuery = PageQuery.Get(pageIndex, pageSize, sorts, keyword, indexFrom, searchFields);
         var defaultSortKeys = RelationalRepositoryHelper<T>.GetSortKeys(MongoDataContext);
         var findFluent = GetPerPageFindFluent(
            MongoDataContext.Collection<T>(),
            MongoDataContext.FindOptions,
            selector,
            pageQuery,
            defaultSortKeys,
            predicate,
            session);
         return new Paged<TResult>(
            await findFluent.ToListAsync(ctok),
            await findFluent.CountDocumentsAsync(ctok),
            pageIndex,
            pageSize,
            pageQuery.IndexFrom);
      }

      private long InternalLongCount(Expression<Func<T, bool>> predicate, IClientSessionHandle session)
      {
         var set = MongoDataContext.Collection<T>();
         var filter = predicate == null ? Builders<T>.Filter.Empty : new ExpressionFilterDefinition<T>(predicate);
         return session == null
            ? set.CountDocuments(filter)
            : set.CountDocuments(session, filter);
      }

      private Task<long> InternalLongCountAsync(
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         var set = MongoDataContext.Collection<T>();
         var filter = predicate == null ? Builders<T>.Filter.Empty : new ExpressionFilterDefinition<T>(predicate);
         return session == null
            ? set.CountDocumentsAsync(filter, cancellationToken: ctok)
            : set.CountDocumentsAsync(session, filter, cancellationToken: ctok);
      }

      private Dictionary<string, object> ProcessCreateAndGetResult(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity) validated,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session)
      {
         var mapped = (validated.MatchValidatedEntity, validated.PropertyNames, validated.InputEntity, existMessageFunc);
         var createError = RelationalRepositoryHelper<T>.IfCreateError(mapped);
         if (createError)
         {
            RepositoryHelper<T>.ThrowCreateErrorExistingEntity(mapped, DefaultExistMessageFunc);
         }

         return CreateAndGetKeyValues(mapped, session);
      }

      private async Task<Dictionary<string, object>> ProcessCreateAndGetResultAsync(
         Task<(T MatchValidatedEntity, string[] PropertyNames, T InputEntity)> asyncValidated,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         var validated = await asyncValidated;
         var mapped = (validated.MatchValidatedEntity, validated.PropertyNames, validated.InputEntity, existMessageFunc);
         var createError = RelationalRepositoryHelper<T>.IfCreateError(mapped);
         if (createError)
         {
            RepositoryHelper<T>.ThrowCreateErrorExistingEntity(mapped, DefaultExistMessageFunc);
         }

         return await CreateAndGetKeyValuesAsync(mapped, session, ctok);
      }

      private bool ProcessDeleteAndGetResult((T Entity, T Exist) validated, IClientSessionHandle session)
      {
         if (validated.Exist != null) return DeleteEntry(validated.Exist, session);
         var properties = PropertyHelper.GetProperties(validated.Entity)
            .Where(x => x.GetValue(validated.Entity) != null)
            .Select(y => y.Name)
            .ToArray();
         throw new KeyNotFoundException(DefaultExistMessageFunc(validated.Entity, properties));
      }

      private async Task<bool> ProcessDeleteAndGetResultAsync(
         Task<(T Entity, T Exist)> asyncValidated,
         IClientSessionHandle session,
         CancellationToken ctok)
      {
         var validated = await asyncValidated;
         if (validated.Exist != null) return await DeleteEntryAsync(validated.Exist, session, ctok);
         var properties = PropertyHelper.GetProperties(validated.Entity)
            .Where(x => x.GetValue(validated.Entity) != null)
            .Select(y => y.Name)
            .ToArray();
         throw new KeyNotFoundException(DefaultExistMessageFunc(validated.Entity, properties));
      }

      private Dictionary<string, object> ProcessUpdateAndGetResult(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity) validated,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         var mapped = (validated.MatchValidatedEntity, validated.PropertyNames, validated.Found, validated.InputEntity, existMessageFunc);
         var updateError = RelationalRepositoryHelper<T>.IfUpdateError(mapped);
         if (updateError) RepositoryHelper<T>.ThrowUpdateError(mapped, DefaultExistMessageFunc);
         return UpdateAndGetKeyValues(mapped, session, fieldSelections);
      }

      private async Task<Dictionary<string, object>> ProcessUpdateAndGetResultAsync(
         Task<(T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity)> asyncValidated,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         CancellationToken ctok,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         var validated = await asyncValidated;
         var mapped = (validated.MatchValidatedEntity, validated.PropertyNames, validated.Found, validated.InputEntity, existMessageFunc);
         var updateError = RelationalRepositoryHelper<T>.IfUpdateError(mapped);
         if (updateError) RepositoryHelper<T>.ThrowUpdateError(mapped, DefaultExistMessageFunc);
         return await UpdateAndGetKeyValuesAsync(mapped, session, ctok, fieldSelections);
      }

      private Dictionary<string, object> UpdateAndGetKeyValues(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity, Func<T, string> MessageFunc) validated,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         var updateSuccess = UpdateEntry(validated.InputEntity, validated.MatchValidatedEntity, session, fieldSelections);
         return updateSuccess ? RepositoryHelper<T>.GetKeysAndValues(validated.PropertyNames, validated.InputEntity) : null;
      }

      private async Task<Dictionary<string, object>> UpdateAndGetKeyValuesAsync(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity, Func<T, string> MessageFunc) validated,
         IClientSessionHandle session,
         CancellationToken ctok,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         var updateSuccess = await UpdateEntryAsync(validated.InputEntity, validated.MatchValidatedEntity, session, ctok, fieldSelections);
         return updateSuccess ? RepositoryHelper<T>.GetKeysAndValues(validated.PropertyNames, validated.InputEntity) : null;
      }

      private bool UpdateEntry(
         T entity,
         T exist,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         var pkPredicate = MongoDataContext.BuildPrimaryKeyPredicate(exist);
         var updateDefinition = GetUpdateDefinitionFromEntity(entity);
         var selectionUpdateDefinition = GetSelectedUpdateDefinition(entity, fieldSelections);
         if (session == null)
         {
            var _ = MongoDataContext.Collection<T>().UpdateOne(pkPredicate.Predicate, selectionUpdateDefinition ?? updateDefinition);
            return _.IsAcknowledged && _.ModifiedCount > 0;
         }

         var __ = MongoDataContext.Collection<T>().UpdateOne(session, pkPredicate.Predicate, selectionUpdateDefinition ?? updateDefinition);
         return __.IsAcknowledged && __.ModifiedCount > 0;
      }

      private async Task<bool> UpdateEntryAsync(
         T entity,
         T exist,
         IClientSessionHandle session,
         CancellationToken ctok,
         params Expression<Func<T, object>>[] fieldSelections)
      {
         var pkPredicate = MongoDataContext.BuildPrimaryKeyPredicate(exist);
         var updateDefinition = GetUpdateDefinitionFromEntity(entity);
         var selectionUpdateDefinition = GetSelectedUpdateDefinition(entity, fieldSelections);
         if (session == null)
         {
            var _ = await MongoDataContext.Collection<T>()
               .UpdateOneAsync(pkPredicate.Predicate, selectionUpdateDefinition ?? updateDefinition, cancellationToken: ctok);
            return _.IsAcknowledged && _.ModifiedCount > 0;
         }

         var __ = await MongoDataContext.Collection<T>()
            .UpdateOneAsync(session, pkPredicate.Predicate, selectionUpdateDefinition ?? updateDefinition, cancellationToken: ctok);
         return __.IsAcknowledged && __.ModifiedCount > 0;
      }

      private static UpdateDefinition<T> GetUpdateDefinitionFromEntity(T entity)
      {
         var dict = entity.ToDictionary();
         if (dict.ContainsKey("Id")) dict.Remove("Id");
         var keys = dict.Keys.ToArray();
         var values = dict.Values.ToArray();
         var firstKey = keys.First();
         var firstValue = values.First();
         var updateDefinition = Builders<T>.Update.Set(firstKey, firstValue);
         for (var i = 1; i <= values.Length - 1; i++)
         {
            updateDefinition = updateDefinition.Set(keys[i], values[i]);
         }

         return updateDefinition;
      }

      #endregion
   }
}