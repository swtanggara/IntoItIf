namespace IntoItIf.Base.Services.Entities.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Args;
   using Base.Helpers;
   using Domain;
   using Domain.Entities;
   using Domain.Options;
   using Exceptions;
   using Interceptors;
   using Repositories;
   using UnitOfWork;

   internal static class EntityServiceHelper
   {
      #region Methods

      internal static Option<Dictionary<string, object>> CreateEntity<T, TDto, TCreateInterceptor, TRepository>(
         this Option<ServiceMapping<T, TCreateInterceptor, TRepository>> serviceMapping,
         Option<TDto> dto)
         where T : class, IEntity
         where TDto : class, IDto
         where TCreateInterceptor : ICreateInterceptor
         where TRepository : class, IRepository<T>
      {
         try
         {
            return serviceMapping.GetCreateParametersMapping(dto)
               .MapFlatten(
                  x =>
                  {
                     x.Args.PreValidation?.Invoke(x.Dto);
                     x.Dto.Validate();
                     x.Args.PostValidation?.Invoke(x.Dto);
                     var entity = x.Dto.ToEntity<T>();
                     AsyncHelper.RunSync(() => x.Args.PostMap?.Invoke(entity, x.Repository));
                     var result = x.Repository.Add(entity, x.Args.MessageIfExistFunc);
                     AsyncHelper.RunSync(() => x.Args.PreSave?.Invoke(entity, x.Repository));
                     if (x.Args.PendingSave) return result;
                     x.Uow.SaveChanges();
                     return result;
                  });
         }
         catch (Exception ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
      }

      internal static async Task<Option<Dictionary<string, object>>> CreateEntityAsync<T, TDto, TCreateInterceptor, TRepository>(
         this Option<ServiceMapping<T, TCreateInterceptor, TRepository>> serviceMapping,
         Option<TDto> dto,
         Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TCreateInterceptor : ICreateInterceptor
         where TRepository : class, IRepository<T>
      {
         try
         {
            return await serviceMapping.GetAsyncCreateParametersMapping(dto, ctok)
               .MapFlattenAsync(
                  async x =>
                  {
                     x.Args.PreValidation?.Invoke(x.Dto);
                     await x.Dto.ValidateAsync();
                     x.Args.PostValidation?.Invoke(x.Dto);
                     var entity = x.Dto.ToEntity<T>();
                     if (x.Args.PostMap != null) await x.Args.PostMap.Invoke(entity, x.Repository);
                     var result = await x.Repository.AddAsync(entity, x.Args.MessageIfExistFunc, x.Ctok);
                     if (x.Args.PreSave != null) await x.Args.PreSave.Invoke(entity, x.Repository);
                     if (x.Args.PendingSave) return result;
                     await x.Uow.SaveChangesAsync(ctok);
                     return result;
                  });
         }
         catch (Exception ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
      }

      internal static Option<bool> DeleteEntity<T, TDto, TDeleteInterceptor, TRepository>(
         this Option<ServiceMapping<T, TDeleteInterceptor, TRepository>> serviceMapping,
         Option<TDto> criteria)
         where T : class, IEntity
         where TDto : class, IDto
         where TDeleteInterceptor : IDeleteInterceptor
         where TRepository : class, IRepository<T>
      {
         try
         {
            return serviceMapping.GetDeleteParametersMapping(criteria)
               .MapFlatten(
                  x =>
                  {
                     var entityCriteria = x.Criteria.ToEntity<T>();
                     AsyncHelper.RunSync(() => x.Args.PreDelete?.Invoke(entityCriteria, x.Repository));
                     var result = x.Repository.Remove(entityCriteria);
                     AsyncHelper.RunSync(() => x.Args.PreSave?.Invoke(entityCriteria, x.Repository));
                     if (x.Args.PendingSave) return result;
                     x.Uow.SaveChanges();
                     return result;
                  });
         }
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      internal static async Task<Option<bool>> DeleteEntityAsync<T, TDto, TDeleteInterceptor, TRepository>(
         this Option<ServiceMapping<T, TDeleteInterceptor, TRepository>> serviceMapping,
         Option<TDto> criteria,
         Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TDeleteInterceptor : IDeleteInterceptor
         where TRepository : class, IRepository<T>
      {
         try
         {
            return await serviceMapping.GetAsyncDeleteParametersMapping(criteria, ctok)
               .MapFlattenAsync(
                  async x =>
                  {
                     var entityCriteria = x.Criteria.ToEntity<T>();
                     if (x.Args.PreDelete != null) await x.Args.PreDelete.Invoke(entityCriteria, x.Repository);
                     var result = await x.Repository.RemoveAsync(entityCriteria, x.Ctok);
                     if (x.Args.PreSave != null) await x.Args.PreSave.Invoke(entityCriteria, x.Repository);
                     if (x.Args.PendingSave) return result;
                     await x.Uow.SaveChangesAsync(x.Ctok);
                     return result;
                  });
         }
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      internal static Option<(IUow Uow, TRepository Repository, SaveInterceptorArgs<T, TDto> Args, TDto Dto, CancellationToken Ctok)>
         GetAsyncCreateParametersMapping<T, TDto, TCreateInterceptor, TRepository>(
            this Option<ServiceMapping<T, TCreateInterceptor, TRepository>> serviceMapping,
            Option<TDto> dto,
            Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TCreateInterceptor : ICreateInterceptor
         where TRepository : class, IRepository<T>
      {
         return serviceMapping.GetCreateParametersMapping(dto)
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  x.Item1.Uow,
                  x.Item1.Repository,
                  x.Item1.Args,
                  x.Item1.Dto,
                  Ctok: x.Item2
               ));
      }

      internal static
         Option<(IUow Uow, TRepository Repository, DeleteInterceptorArgs<T> Args, TDto Criteria, CancellationToken Ctok)>
         GetAsyncDeleteParametersMapping<T, TDto, TDeleteInterceptor, TRepository>(
            this Option<ServiceMapping<T, TDeleteInterceptor, TRepository>> serviceMapping,
            Option<TDto> criteria,
            Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TDeleteInterceptor : IDeleteInterceptor
         where TRepository : class, IRepository<T>
      {
         return serviceMapping.GetDeleteParametersMapping(criteria)
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  x.Item1.Uow,
                  x.Item1.Repository,
                  x.Item1.Args,
                  x.Item1.Criteria,
                  Ctok: x.Item2
               ));
      }

      internal static
         Option<(IUow Uow, TRepository Repository, ReadLookupInterceptorArgs<T> Args, bool UseValueAsId, CancellationToken Ctok)>
         GetAsyncReadLookupParametersMapping<T, TReadLookupInterceptor, TRepository>(
            this Option<ServiceMapping<T, TReadLookupInterceptor, TRepository>> serviceMapping,
            Option<bool> useValueAsId,
            Option<CancellationToken> ctok)
         where T : class, IEntity
         where TReadLookupInterceptor : IReadLookupInterceptor
         where TRepository : class, IRepository<T>
      {
         return serviceMapping.GetReadLookupParametersMapping(useValueAsId)
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  x.Item1.Uow,
                  x.Item1.Repository,
                  x.Item1.Args,
                  x.Item1.UseValueAsId,
                  Ctok: x.Item2));
      }

      internal static
         Option<(IUow Uow, TRepository Repository, ReadOneInterceptorArgs<T> Args, TDto Criteria, CancellationToken Ctok)>
         GetAsyncReadOneParametersMapping<T, TDto, TReadOneInterceptor, TRepository>(
            this Option<ServiceMapping<T, TReadOneInterceptor, TRepository>> serviceMapping,
            Option<TDto> criteria,
            Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadOneInterceptor : IReadOneInterceptor
         where TRepository : class, IRepository<T>
      {
         return serviceMapping.GetReadOneParametersMapping(criteria)
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  x.Item1.Uow,
                  x.Item1.Repository,
                  x.Item1.Args,
                  x.Item1.Criteria,
                  Ctok: x.Item2
               ));
      }

      internal static
         Option<(IUow Uow, TRepository Repository, ReadPagedInterceptorArgs<T> Args, int PageNo, int PageSize, string[] Sorts, string
            Keyword, CancellationToken Ctok)> GetAsyncReadPagedParametersMapping<T, TReadPagedInterceptor, TRepository>(
            this Option<ServiceMapping<T, TReadPagedInterceptor, TRepository>> serviceMapping,
            Option<int> pageNo,
            Option<int> pageSize,
            Option<string>[] sorts,
            Option<string> keyword,
            Option<CancellationToken> ctok)
         where T : class, IEntity
         where TReadPagedInterceptor : IReadPagedInterceptor
         where TRepository : class, IRepository<T>
      {
         return serviceMapping.GetReadPagedParametersMapping(pageNo, pageSize, sorts, keyword)
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  x.Item1.Uow,
                  x.Item1.Repository,
                  x.Item1.Args,
                  x.Item1.PageNo,
                  x.Item1.PageSize,
                  x.Item1.Sorts,
                  x.Item1.Keyword,
                  Ctok: x.Item2
               ));
      }

      internal static
         Option<(IUow Uow, TRepository Repository, SaveInterceptorArgs<T, TDto> Args, TDto Dto, CancellationToken Ctok)>
         GetAsyncUpdateParametersMapping<T, TDto, TUpdateInterceptor, TRepository>(
            this Option<ServiceMapping<T, TUpdateInterceptor, TRepository>> serviceMapping,
            Option<TDto> dto,
            Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TUpdateInterceptor : IUpdateInterceptor
         where TRepository : class, IRepository<T>
      {
         return serviceMapping.GetUpdateParametersMapping(dto)
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  x.Item1.Uow,
                  x.Item1.Repository,
                  x.Item1.Args,
                  x.Item1.Dto,
                  Ctok: x.Item2
               ));
      }

      internal static Option<TDto> GetByPredicate<T, TDto, TReadOneInterceptor, TRepository>(
         this Option<ServiceMapping<T, TReadOneInterceptor, TRepository>> serviceMapping,
         Option<TDto> criteria)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadOneInterceptor : IReadOneInterceptor
         where TRepository : class, IRepository<T>
      {
         try
         {
            return serviceMapping.GetReadOneParametersMapping(criteria)
               .MapFlatten(
                  x =>
                  {
                     var tempEntity = x.Criteria.ToEntity<T>();
                     var entityPredicate = tempEntity.BuildPredicate();
                     var predicate = OptionPredicateBuilder.New(entityPredicate);
                     AsyncHelper.RunSync(() => x.Args.OneValidation?.Invoke(tempEntity, x.Repository));
                     if (x.Args.Predicate != null) predicate = predicate.And(x.Args.Predicate);
                     var realPredicate = predicate.ReduceOrDefault();
                     var entity = x.Repository.GetFirstOrDefault(realPredicate);
                     if (x.Args.IsView)
                     {
                        if (x.Repository is BaseRelationalRepository<T> relationalRepo)
                        {
                           entity = relationalRepo.GetFirstOrDefault(realPredicate, x.Args.OrderBy);
                        }
                     }
                     else if (x.Repository is BaseRelationalRepository<T> relationalRepo)
                     {
                        entity = relationalRepo.GetFirstOrDefault(realPredicate, x.Args.OrderBy, x.Args.Include);
                     }

                     return !entity.IsSome()
                        ? Fail<TDto>.Throw(new RetrieveValidationException("Entity not found by specified criteria"))
                        : entity.ToDto<T, TDto>();
                  });
         }
         catch (Exception ex)
         {
            return Fail<TDto>.Throw(ex);
         }
      }

      internal static async Task<Option<TDto>> GetByPredicateAsync<T, TDto, TReadOneInterceptor, TRepository>(
         this Option<ServiceMapping<T, TReadOneInterceptor, TRepository>> serviceMapping,
         Option<TDto> criteria,
         Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadOneInterceptor : IReadOneInterceptor
         where TRepository : class, IRepository<T>
      {
         try
         {
            return await serviceMapping.GetAsyncReadOneParametersMapping(criteria, ctok)
               .MapFlattenAsync(
                  async x =>
                  {
                     var tempEntity = x.Criteria.ToEntity<T>();
                     var entityPredicate = ObjectDictionaryHelpers.BuildPredicate<T>(tempEntity);
                     var predicate = OptionPredicateBuilder.New<T>(entityPredicate);
                     if (x.Args.OneValidation != null) await x.Args.OneValidation.Invoke(tempEntity, x.Repository);
                     if (x.Args.Predicate != null) predicate = predicate.And(x.Args.Predicate);
                     var realPredicate = predicate.ReduceOrDefault();
                     var entity = x.Repository.GetFirstOrDefault(realPredicate);
                     if (x.Args.IsView)
                     {
                        if (x.Repository is BaseRelationalRepository<T> relationalRepo)
                        {
                           entity = await relationalRepo.GetFirstOrDefaultAsync(realPredicate, x.Args.OrderBy, x.Ctok);
                        }
                     }
                     else if (x.Repository is BaseRelationalRepository<T> relationalRepo)
                     {
                        entity = await relationalRepo.GetFirstOrDefaultAsync(realPredicate, x.Args.OrderBy, x.Args.Include, x.Ctok);
                     }

                     return !entity.IsSome()
                        ? Fail<TDto>.Throw(new RetrieveValidationException("Entity not found by specified criteria"))
                        : entity.ToDto<T, TDto>();
                  });
         }
         catch (Exception ex)
         {
            return Fail<TDto>.Throw(ex);
         }
      }

      internal static Option<(IUow Uow, TRepository Repository, SaveInterceptorArgs<T, TDto> Args, TDto
            Dto)>
         GetCreateParametersMapping<T, TDto, TCreateInterceptor, TRepository>(
            this Option<ServiceMapping<T, TCreateInterceptor, TRepository>> serviceMapping,
            Option<TDto> dto)
         where T : class, IEntity
         where TDto : class, IDto
         where TCreateInterceptor : ICreateInterceptor
         where TRepository : class, IRepository<T>
      {
         return serviceMapping
            .Combine(dto)
            .Map(
               x => (
                  x.Item1.Uow.ReduceOrDefault(),
                  x.Item1.Repository.ReduceOrDefault(),
                  Args: x.Item1.Interceptor.ReduceOrDefault().OpenForCreate<T, TDto>(),
                  Dto: x.Item2
               ));
      }

      internal static
         Option<(IUow Uow, TRepository Repository, DeleteInterceptorArgs<T> Args, TDto Criteria)>
         GetDeleteParametersMapping<T, TDto, TDeleteInterceptor, TRepository>(
            this Option<ServiceMapping<T, TDeleteInterceptor, TRepository>> serviceMapping,
            Option<TDto> criteria)
         where T : class, IEntity
         where TDto : class, IDto
         where TDeleteInterceptor : IDeleteInterceptor
         where TRepository : class, IRepository<T>
      {
         return serviceMapping
            .Combine(criteria)
            .Map(
               x => (
                  x.Item1.Uow.ReduceOrDefault(),
                  x.Item1.Repository.ReduceOrDefault(),
                  Args: x.Item1.Interceptor.ReduceOrDefault().OpenForDelete<T>(),
                  Criteria: x.Item2
               ));
      }

      internal static Option<List<KeyValue>> GetLookups<T, TReadLookupInterceptor, TRepository>(
         this Option<ServiceMapping<T, TReadLookupInterceptor, TRepository>> serviceMapping,
         Option<bool> useValueAsId)
         where T : class, IEntity
         where TReadLookupInterceptor : IReadLookupInterceptor
         where TRepository : class, IRepository<T>
      {
         try
         {
            return serviceMapping.GetReadLookupParametersMapping(useValueAsId)
               .MapFlatten(
                  x =>
                  {
                     if (string.IsNullOrWhiteSpace(x.Args.IdProperty))
                        throw new InvalidOperationException("ReadLookupInterceptorArgs.IdProperty cannot be null");
                     if (string.IsNullOrWhiteSpace(x.Args.ValueProperty))
                        throw new InvalidOperationException("ReadLookupInterceptorArgs.ValueProperty cannot be null");
                     AsyncHelper.RunSync(() => x.Args.LookupValidation?.Invoke(x.Repository));
                     var result = x.Repository.GetLookups(
                        x.Args.IdProperty,
                        x.Args.ValueProperty,
                        x.UseValueAsId,
                        x.Args.Predicate);
                     if (x.Args.IsView)
                     {
                        if (x.Repository is BaseRelationalRepository<T> relationalRepo)
                        {
                           result = relationalRepo.GetLookups(x.Args.IdProperty, x.Args.ValueProperty, x.UseValueAsId, x.Args.Predicate);
                        }
                     }
                     else if (x.Repository is BaseRelationalRepository<T> relationalRepo)
                     {
                        result = relationalRepo.GetLookups(
                           x.Args.IdProperty,
                           x.Args.ValueProperty,
                           x.UseValueAsId,
                           x.Args.Predicate,
                           x.Args.Include);
                     }

                     return result;
                  });
         }
         catch (InvalidOperationException ex)
         {
            return Fail<List<KeyValue>>.Throw(ex);
         }
         catch (RetrieveValidationException ex)
         {
            return Fail<List<KeyValue>>.Throw(ex);
         }
         catch (Exception ex)
         {
            return Fail<List<KeyValue>>.Throw(ex);
         }
      }

      internal static async Task<Option<List<KeyValue>>> GetLookupsAsync<T, TReadLookupInterceptor, TRepository>(
         this Option<ServiceMapping<T, TReadLookupInterceptor, TRepository>> serviceMapping,
         Option<bool> useValueAsId,
         Option<CancellationToken> ctok)
         where T : class, IEntity
         where TReadLookupInterceptor : IReadLookupInterceptor
         where TRepository : class, IRepository<T>
      {
         try
         {
            return await serviceMapping.GetAsyncReadLookupParametersMapping(useValueAsId, ctok)
               .MapFlattenAsync(
                  async x =>
                  {
                     if (string.IsNullOrWhiteSpace(x.Args.IdProperty))
                        throw new InvalidOperationException("ReadLookupInterceptorArgs.IdProperty cannot be null");
                     if (string.IsNullOrWhiteSpace(x.Args.ValueProperty))
                        throw new InvalidOperationException("ReadLookupInterceptorArgs.ValueProperty cannot be null");
                     if (x.Args.LookupValidation != null) await x.Args.LookupValidation.Invoke(x.Repository);
                     var result = await x.Repository.GetLookupsAsync(
                        x.Args.IdProperty,
                        x.Args.ValueProperty,
                        x.UseValueAsId,
                        x.Args.Predicate,
                        ctok);
                     if (x.Args.IsView)
                     {
                        if (x.Repository is BaseRelationalRepository<T> relationalRepo)
                        {
                           result = await relationalRepo.GetLookupsAsync(
                              x.Args.IdProperty,
                              x.Args.ValueProperty,
                              x.UseValueAsId,
                              x.Args.Predicate,
                              ctok);
                        }
                     }
                     else if (x.Repository is BaseRelationalRepository<T> relationalRepo)
                     {
                        result = await relationalRepo.GetLookupsAsync(
                           x.Args.IdProperty,
                           x.Args.ValueProperty,
                           x.UseValueAsId,
                           x.Args.Predicate,
                           x.Args.Include,
                           ctok);
                     }

                     return result;
                  });
         }
         catch (InvalidOperationException ex)
         {
            return Fail<List<KeyValue>>.Throw(ex);
         }
         catch (RetrieveValidationException ex)
         {
            return Fail<List<KeyValue>>.Throw(ex);
         }
         catch (Exception ex)
         {
            return Fail<List<KeyValue>>.Throw(ex);
         }
      }

      internal static Option<IPaged<TDto>> GetPaged<T, TDto, TReadPagedInterceptor, TRepository>(
         this Option<ServiceMapping<T, TReadPagedInterceptor, TRepository>> serviceMapping,
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadPagedInterceptor : IReadPagedInterceptor
         where TRepository : class, IRepository<T>
      {
         try
         {
            return serviceMapping.GetReadPagedParametersMapping(pageNo, pageSize, sorts, keyword)
               .MapFlatten(
                  x =>
                  {
                     AsyncHelper.RunSync(() => x.Args.PagedValidation?.Invoke(x.Repository));
                     Expression<Func<T, TDto>> toDtoExpr = y => y.ToDto<TDto>().ReduceOrDefault();
                     if (!(x.Repository is BaseRelationalRepository<T> relationalRepo))
                     {
                        return x.Repository.GetPaged<TDto>(
                           toDtoExpr,
                           x.Args.SearchFields,
                           x.PageNo,
                           x.PageSize,
                           x.Sorts,
                           x.Keyword,
                           x.Args.DefaultPageIndexFrom,
                           x.Args.Predicate);
                     }

                     return relationalRepo.GetPaged<TDto>(
                        toDtoExpr,
                        x.Args.SearchFields,
                        x.PageNo,
                        x.PageSize,
                        x.Sorts,
                        x.Keyword,
                        x.Args.DefaultPageIndexFrom,
                        x.Args.Predicate,
                        x.Args.IsView ? null : x.Args.Include);
                  });
         }
         catch (ArgumentNullException ex)
         {
            return Fail<IPaged<TDto>>.Throw(ex);
         }
         catch (RetrieveValidationException ex)
         {
            return Fail<IPaged<TDto>>.Throw(ex);
         }
         catch (Exception ex)
         {
            return Fail<IPaged<TDto>>.Throw(ex);
         }
      }

      internal static async Task<Option<IPaged<TDto>>> GetPagedAsync<T, TDto, TReadPagedInterceptor, TRepository>(
         this Option<ServiceMapping<T, TReadPagedInterceptor, TRepository>> serviceMapping,
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadPagedInterceptor : IReadPagedInterceptor
         where TRepository : class, IRepository<T>
      {
         try
         {
            return await serviceMapping.GetAsyncReadPagedParametersMapping(pageNo, pageSize, sorts, keyword, ctok)
               .MapFlattenAsync(
                  async x =>
                  {
                     if (x.Args.PagedValidation != null) await x.Args.PagedValidation.Invoke(x.Repository);
                     Expression<Func<T, TDto>> toDtoExpr = y => y.ToDto<TDto>().ReduceOrDefault();
                     if (!(x.Repository is BaseRelationalRepository<T> relationalRepo))
                     {
                        return x.Repository.GetPaged<TDto>(
                           toDtoExpr,
                           x.Args.SearchFields,
                           x.PageNo,
                           x.PageSize,
                           x.Sorts,
                           x.Keyword,
                           x.Args.DefaultPageIndexFrom,
                           x.Args.Predicate);
                     }

                     return await relationalRepo.GetPagedAsync<TDto>(
                        toDtoExpr,
                        x.Args.SearchFields,
                        x.PageNo,
                        x.PageSize,
                        x.Sorts,
                        x.Keyword,
                        x.Args.DefaultPageIndexFrom,
                        x.Args.Predicate,
                        x.Args.IsView ? null : x.Args.Include,
                        ctok);
                  });
         }
         catch (ArgumentNullException ex)
         {
            return Fail<IPaged<TDto>>.Throw(ex);
         }
         catch (RetrieveValidationException ex)
         {
            return Fail<IPaged<TDto>>.Throw(ex);
         }
         catch (Exception ex)
         {
            return Fail<IPaged<TDto>>.Throw(ex);
         }
      }

      internal static Option<(IUow Uow, TRepository Repository, ReadLookupInterceptorArgs<T> Args, bool UseValueAsId)>
         GetReadLookupParametersMapping<T, TReadLookupInterceptor, TRepository>(
            this Option<ServiceMapping<T, TReadLookupInterceptor, TRepository>> serviceMapping,
            Option<bool> useValueAsId)
         where T : class, IEntity
         where TReadLookupInterceptor : IReadLookupInterceptor
         where TRepository : class, IRepository<T>
      {
         return serviceMapping
            .Combine(useValueAsId, true, true)
            .Map(
               x => (
                  x.Item1.Uow.ReduceOrDefault(),
                  x.Item1.Repository.ReduceOrDefault(),
                  Args: x.Item1.Interceptor.ReduceOrDefault().OpenForReadLookup<T>(),
                  UseValueAsId: x.Item2
               ));
      }

      internal static
         Option<(IUow Uow, TRepository Repository, ReadOneInterceptorArgs<T> Args, TDto Criteria)>
         GetReadOneParametersMapping<T, TDto, TReadOneInterceptor, TRepository>(
            this Option<ServiceMapping<T, TReadOneInterceptor, TRepository>> serviceMapping,
            Option<TDto> criteria)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadOneInterceptor : IReadOneInterceptor
         where TRepository : class, IRepository<T>
      {
         return serviceMapping
            .Combine(criteria)
            .Map(
               x => (
                  x.Item1.Uow.ReduceOrDefault(),
                  x.Item1.Repository.ReduceOrDefault(),
                  Args: x.Item1.Interceptor.ReduceOrDefault().OpenForReadOne<T>(),
                  Criteria: x.Item2
               ));
      }

      internal static
         Option<(IUow Uow, TRepository Repository, ReadPagedInterceptorArgs<T> Args, int PageNo, int
            PageSize,
            string[] Sorts, string Keyword)> GetReadPagedParametersMapping<T, TReadPagedInterceptor, TRepository>(
            this Option<ServiceMapping<T, TReadPagedInterceptor, TRepository>> serviceMapping,
            Option<int> pageNo,
            Option<int> pageSize,
            Option<string>[] sorts,
            Option<string> keyword)
         where T : class, IEntity
         where TReadPagedInterceptor : IReadPagedInterceptor
         where TRepository : class, IRepository<T>
      {
         return serviceMapping
            .Combine(pageNo, true, PageQuery.DefaultIndexFrom.Id)
            .Combine(pageSize, true, PageQuery.DefaultPageSize)
            .Combine(sorts.ToOptionOfEnumerable(), true)
            .Combine(keyword, true)
            .Map(
               x => (
                  x.Item1.Item1.Item1.Item1.Uow.ReduceOrDefault(),
                  x.Item1.Item1.Item1.Item1.Repository.ReduceOrDefault(),
                  Args: x.Item1.Item1.Item1.Item1.Interceptor.ReduceOrDefault().OpenForReadPaged<T>(),
                  PageNo: x.Item1.Item1.Item1.Item2,
                  PageSize: x.Item1.Item1.Item2,
                  Sorts: x.Item1.Item2.ToArray(),
                  Keyword: x.Item2
               ));
      }

      internal static Option<(IUow Uow, TRepository Repository, SaveInterceptorArgs<T, TDto> Args, TDto Dto)>
         GetUpdateParametersMapping<T, TDto, TUpdateInterceptor, TRepository>(
            this Option<ServiceMapping<T, TUpdateInterceptor, TRepository>> serviceMapping,
            Option<TDto> dto)
         where T : class, IEntity
         where TDto : class, IDto
         where TUpdateInterceptor : IUpdateInterceptor
         where TRepository : class, IRepository<T>
      {
         return serviceMapping
            .Combine(dto)
            .Map(
               x => (
                  x.Item1.Uow.ReduceOrDefault(),
                  x.Item1.Repository.ReduceOrDefault(),
                  Args: x.Item1.Interceptor.ReduceOrDefault().OpenForUpdate<T, TDto>(),
                  Dto: x.Item2
               ));
      }

      internal static Option<Dictionary<string, object>> UpdateEntity<T, TDto, TUpdateInterceptor, TRepository>(
         this Option<ServiceMapping<T, TUpdateInterceptor, TRepository>> serviceMapping,
         Option<TDto> dto)
         where T : class, IEntity
         where TDto : class, IDto
         where TUpdateInterceptor : IUpdateInterceptor
         where TRepository : class, IRepository<T>
      {
         try
         {
            return serviceMapping.GetUpdateParametersMapping(dto)
               .MapFlatten(
                  x =>
                  {
                     x.Args.PreValidation?.Invoke(x.Dto);
                     x.Dto.Validate();
                     x.Args.PostValidation?.Invoke(x.Dto);
                     var entity = x.Dto.ToEntity<T>();
                     AsyncHelper.RunSync(() => x.Args.PostMap?.Invoke(entity, x.Repository));
                     var result = x.Repository.Change(entity, x.Args.MessageIfExistFunc);
                     AsyncHelper.RunSync(() => x.Args.PreSave?.Invoke(entity, x.Repository));
                     if (x.Args.PendingSave) return result;
                     x.Uow.SaveChanges();
                     return result;
                  });
         }
         catch (Exception ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
      }

      internal static async Task<Option<Dictionary<string, object>>> UpdateEntityAsync<T, TDto, TUpdateInterceptor, TRepository>(
         this Option<ServiceMapping<T, TUpdateInterceptor, TRepository>> serviceMapping,
         Option<TDto> dto,
         Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TUpdateInterceptor : IUpdateInterceptor
         where TRepository : class, IRepository<T>
      {
         try
         {
            return await serviceMapping.GetAsyncUpdateParametersMapping(dto, ctok)
               .MapFlattenAsync(
                  async x =>
                  {
                     x.Args.PreValidation?.Invoke(x.Dto);
                     await x.Dto.ValidateAsync();
                     x.Args.PostValidation?.Invoke(x.Dto);
                     var entity = x.Dto.ToEntity<T>();
                     if (x.Args.PostMap != null) await x.Args.PostMap.Invoke(entity, x.Repository);
                     var result = await x.Repository.ChangeAsync(entity, x.Args.MessageIfExistFunc, ctok);
                     if (x.Args.PreSave != null) await x.Args.PreSave.Invoke(entity, x.Repository);
                     if (x.Args.PendingSave) return result;
                     await x.Uow.SaveChangesAsync(ctok);
                     return result;
                  });
         }
         catch (Exception ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
      }

      #endregion
   }
}