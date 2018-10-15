namespace IntoItIf.Dsl.Entities.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Args;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Core.Exceptions;
   using Core.Helpers;
   using Dal.Exceptions;
   using Dal.Exceptions.Helpers;
   using Dal.Helpers;
   using Dal.UnitOfWorks;
   using Exceptions;
   using Interceptors;

   internal static class EntityServiceHelper
   {
      #region Methods

      internal static Option<TInterceptor> As<TInterceptor>(this IInterceptor interceptor)
         where TInterceptor : IInterceptor
      {
         return (TInterceptor)interceptor;
      }

      internal static Option<ServiceMapping<T, TInterceptor2>> ChangeInterceptor<
         T, TInterceptor1, TInterceptor2>(
         this Option<ServiceMapping<T, TInterceptor1>> serviceMapping)
         where T : class, IEntity
         where TInterceptor1 : IInterceptor
         where TInterceptor2 : IInterceptor
      {
         try
         {
            return serviceMapping.ReduceOrDefault().ChangeInterceptor<TInterceptor2>();
         }
         catch (Exception ex)
         {
            return Fail<ServiceMapping<T, TInterceptor2>>.Throw(ex);
         }
      }

      internal static Option<Dictionary<string, object>> CreateEntity<T, TDto, TCreateInterceptor>(
         this Option<ServiceMapping<T, TCreateInterceptor>> serviceMapping,
         Option<TDto> dto)
         where T : class, IEntity
         where TDto : class, IDto
         where TCreateInterceptor : ICreateInterceptor
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
                     var result = x.Repository.Create(entity, x.Args.MessageIfExistFunc);
                     AsyncHelper.RunSync(() => x.Args.PreSave?.Invoke(entity, x.Repository));
                     if (x.Args.PendingSave) return result;
                     x.Uow.SaveChanges();
                     return result;
                  });
         }
         catch (ArgumentNullException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (ValidationException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PreValidationException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PostValidationException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PostMapException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PreSaveException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (ExistingEntityException<T> ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
#if NETSTANDARD2_0
         catch (EfCoreDbUpdateException ex)
         {
            var message = ex.HumanizeKeyError();
            return Fail<Dictionary<string, object>>.Throw(new EfCoreDbUpdateException(message, ex));
         }
#endif
#if NET471
         catch (EfDbUpdateException ex)
         {
            var message = ex.HumanizeKeyError();
            return Fail<Dictionary<string, object>>.Throw(new EfDbUpdateException(message, ex));
         }
#endif
         catch (Exception ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
      }

      internal static async Task<Option<Dictionary<string, object>>> CreateEntityAsync<T, TDto, TCreateInterceptor>(
         this Option<ServiceMapping<T, TCreateInterceptor>> serviceMapping,
         Option<TDto> dto,
         Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TCreateInterceptor : ICreateInterceptor
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
                     var result = await x.Repository.CreateAsync(entity, x.Args.MessageIfExistFunc, x.Ctok);
                     if (x.Args.PreSave != null) await x.Args.PreSave.Invoke(entity, x.Repository);
                     if (x.Args.PendingSave) return result;
                     await x.Uow.SaveChangesAsync(ctok);
                     return result;
                  });
         }
         catch (ArgumentNullException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (ValidationException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PreValidationException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PostValidationException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PostMapException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PreSaveException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (ExistingEntityException<T> ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
#if NETSTANDARD2_0
         catch (EfCoreDbUpdateException ex)
         {
            var message = ex.HumanizeKeyError();
            return Fail<Dictionary<string, object>>.Throw(new EfCoreDbUpdateException(message, ex));
         }
#endif
#if NET471
         catch (EfDbUpdateException ex)
         {
            var message = ex.HumanizeKeyError();
            return Fail<Dictionary<string, object>>.Throw(new EfDbUpdateException(message, ex));
         }
#endif
         catch (Exception ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
      }

      internal static Option<bool> DeleteEntity<T, TDto, TDeleteInterceptor>(
         this Option<ServiceMapping<T, TDeleteInterceptor>> serviceMapping,
         Option<TDto> criteria)
         where T : class, IEntity
         where TDto : class, IDto
         where TDeleteInterceptor : IDeleteInterceptor
      {
         try
         {
            return serviceMapping.GetDeleteParametersMapping(criteria)
               .MapFlatten(
                  x =>
                  {
                     var entityCriteria = x.Criteria.ToEntity<T>();
                     AsyncHelper.RunSync(() => x.Args.PreDelete?.Invoke(entityCriteria, x.Repository));
                     var result = x.Repository.Delete(entityCriteria);
                     AsyncHelper.RunSync(() => x.Args.PreSave?.Invoke(entityCriteria, x.Repository));
                     if (x.Args.PendingSave) return result;
                     x.Uow.SaveChanges();
                     return result;
                  });
         }
         catch (ArgumentNullException ex)
         {
            return Fail<bool>.Throw(ex);
         }
         catch (KeyNotFoundException ex)
         {
            return Fail<bool>.Throw(ex);
         }
         catch (PreDeleteException ex)
         {
            return Fail<bool>.Throw(ex);
         }
         catch (PreSaveException ex)
         {
            return Fail<bool>.Throw(ex);
         }
#if NETSTANDARD2_0
         catch (EfCoreDbUpdateException ex)
         {
            return Fail<bool>.Throw(ex);
         }
#endif
#if NET471
         catch (EfDbUpdateException ex)
         {
            return Fail<bool>.Throw(ex);
         }
#endif
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      internal static async Task<Option<bool>> DeleteEntityAsync<T, TDto, TDeleteInterceptor>(
         this Option<ServiceMapping<T, TDeleteInterceptor>> serviceMapping,
         Option<TDto> criteria,
         Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TDeleteInterceptor : IDeleteInterceptor
      {
         try
         {
            return await serviceMapping.GetAsyncDeleteParametersMapping(criteria, ctok)
               .MapFlattenAsync(
                  async x =>
                  {
                     var entityCriteria = x.Criteria.ToEntity<T>();
                     if (x.Args.PreDelete != null) await x.Args.PreDelete.Invoke(entityCriteria, x.Repository);
                     var result = await x.Repository.DeleteAsync(entityCriteria, x.Ctok);
                     if (x.Args.PreSave != null) await x.Args.PreSave.Invoke(entityCriteria, x.Repository);
                     if (x.Args.PendingSave) return result;
                     await x.Uow.SaveChangesAsync(x.Ctok);
                     return result;
                  });
         }
         catch (ArgumentNullException ex)
         {
            return Fail<bool>.Throw(ex);
         }
         catch (KeyNotFoundException ex)
         {
            return Fail<bool>.Throw(ex);
         }
         catch (PreDeleteException ex)
         {
            return Fail<bool>.Throw(ex);
         }
         catch (PreSaveException ex)
         {
            return Fail<bool>.Throw(ex);
         }
#if NETSTANDARD2_0
         catch (EfCoreDbUpdateException ex)
         {
            return Fail<bool>.Throw(ex);
         }
#endif
#if NET471
         catch (EfDbUpdateException ex)
         {
            return Fail<bool>.Throw(ex);
         }
#endif
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      internal static Option<(BaseUnitOfWork Uow, BaseRepository<T> Repository, SaveInterceptorArgs<T, TDto> Args, TDto Dto,
            CancellationToken Ctok)>
         GetAsyncCreateParametersMapping<T, TDto, TCreateInterceptor>(
            this Option<ServiceMapping<T, TCreateInterceptor>> serviceMapping,
            Option<TDto> dto,
            Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TCreateInterceptor : ICreateInterceptor
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
         Option<(BaseUnitOfWork Uow, BaseRepository<T> Repository, DeleteInterceptorArgs<T> Args, TDto Criteria,
            CancellationToken Ctok)> GetAsyncDeleteParametersMapping<T, TDto, TDeleteInterceptor>(
            this Option<ServiceMapping<T, TDeleteInterceptor>> serviceMapping,
            Option<TDto> criteria,
            Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TDeleteInterceptor : IDeleteInterceptor
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
         Option<(BaseUnitOfWork Uow, BaseRepository<T> Repository, ReadLookupInterceptorArgs<T> Args, bool UseValueAsId,
            CancellationToken Ctok)> GetAsyncReadLookupParametersMapping<T, TReadLookupInterceptor>(
            this Option<ServiceMapping<T, TReadLookupInterceptor>> serviceMapping,
            Option<bool> useValueAsId,
            Option<CancellationToken> ctok)
         where T : class, IEntity
         where TReadLookupInterceptor : IReadLookupInterceptor
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
         Option<(BaseUnitOfWork Uow, BaseRepository<T> Repository, ReadOneInterceptorArgs<T> Args, TDto Criteria,
            CancellationToken Ctok)> GetAsyncReadOneParametersMapping<T, TDto, TReadOneInterceptor>(
            this Option<ServiceMapping<T, TReadOneInterceptor>> serviceMapping,
            Option<TDto> criteria,
            Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadOneInterceptor : IReadOneInterceptor
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
         Option<(BaseUnitOfWork Uow, BaseRepository<T> Repository, ReadPagedInterceptorArgs<T> Args, int PageNo, int PageSize,
            string[] Sorts, string Keyword, CancellationToken Ctok)> GetAsyncReadPagedParametersMapping<T, TReadPagedInterceptor>(
            this Option<ServiceMapping<T, TReadPagedInterceptor>> serviceMapping,
            Option<int> pageNo,
            Option<int> pageSize,
            Option<string>[] sorts,
            Option<string> keyword,
            Option<CancellationToken> ctok)
         where T : class, IEntity
         where TReadPagedInterceptor : IReadPagedInterceptor
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
         Option<(BaseUnitOfWork Uow, BaseRepository<T> Repository, SaveInterceptorArgs<T, TDto> Args, TDto Dto, CancellationToken
            Ctok)> GetAsyncUpdateParametersMapping<T, TDto, TUpdateInterceptor>(
            this Option<ServiceMapping<T, TUpdateInterceptor>> serviceMapping,
            Option<TDto> dto,
            Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TUpdateInterceptor : IUpdateInterceptor
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

      internal static Option<TDto> GetByPredicate<T, TDto, TReadOneInterceptor>(
         this Option<ServiceMapping<T, TReadOneInterceptor>> serviceMapping,
         Option<TDto> criteria)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadOneInterceptor : IReadOneInterceptor
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
                     Option<T> entity;
                     if (x.Args.IsView)
                     {
                        entity = x.Repository.GetFirstOrDefault(
                           predicate,
                           None.Value,
                           x.Args.OrderBy,
                           true);
                     }
                     else
                     {
                        entity = x.Repository.GetFirstOrDefault(
                           predicate,
                           x.Args.Include,
                           x.Args.OrderBy,
                           true);
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

      internal static async Task<Option<TDto>> GetByPredicateAsync<T, TDto, TReadOneInterceptor>(
         this Option<ServiceMapping<T, TReadOneInterceptor>> serviceMapping,
         Option<TDto> criteria,
         Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadOneInterceptor : IReadOneInterceptor
      {
         try
         {
            return await serviceMapping.GetAsyncReadOneParametersMapping(criteria, ctok)
               .MapFlattenAsync(
                  async x =>
                  {
                     var tempEntity = x.Criteria.ToEntity<T>();
                     var entityPredicate = tempEntity.BuildPredicate();
                     var predicate = OptionPredicateBuilder.New(entityPredicate);
                     if (x.Args.OneValidation != null) await x.Args.OneValidation.Invoke(tempEntity, x.Repository);
                     if (x.Args.Predicate != null) predicate = predicate.And(x.Args.Predicate);
                     Option<T> entity;
                     if (x.Args.IsView)
                     {
                        entity = await x.Repository.GetFirstOrDefaultAsync(
                           predicate,
                           None.Value,
                           x.Args.OrderBy,
                           true,
                           x.Ctok);
                     }
                     else
                     {
                        entity = await x.Repository.GetFirstOrDefaultAsync(
                           predicate,
                           x.Args.Include,
                           x.Args.OrderBy,
                           true,
                           x.Ctok);
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

      internal static Option<(BaseUnitOfWork Uow, BaseRepository<T> Repository, SaveInterceptorArgs<T, TDto> Args, TDto Dto)>
         GetCreateParametersMapping<T, TDto, TCreateInterceptor>(
            this Option<ServiceMapping<T, TCreateInterceptor>> serviceMapping,
            Option<TDto> dto)
         where T : class, IEntity
         where TDto : class, IDto
         where TCreateInterceptor : ICreateInterceptor
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
         Option<(BaseUnitOfWork Uow, BaseRepository<T> Repository, DeleteInterceptorArgs<T> Args, TDto Criteria)>
         GetDeleteParametersMapping<T, TDto, TDeleteInterceptor>(
            this Option<ServiceMapping<T, TDeleteInterceptor>> serviceMapping,
            Option<TDto> criteria)
         where T : class, IEntity
         where TDto : class, IDto
         where TDeleteInterceptor : IDeleteInterceptor
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

      internal static Option<List<KeyValue>> GetLookups<T, TReadLookupInterceptor>(
         this Option<ServiceMapping<T, TReadLookupInterceptor>> serviceMapping,
         Option<bool> useValueAsId)
         where T : class, IEntity
         where TReadLookupInterceptor : IReadLookupInterceptor
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
                     Option<List<KeyValue>> result;
                     if (x.Args.IsView)
                        result = x.Repository.GetLookups(
                           x.Args.IdProperty,
                           x.Args.ValueProperty,
                           x.UseValueAsId,
                           x.Args.Predicate,
                           None.Value,
                           true);
                     else
                        result = x.Repository.GetLookups(
                           x.Args.IdProperty,
                           x.Args.ValueProperty,
                           x.UseValueAsId,
                           x.Args.Predicate,
                           x.Args.Include,
                           true);
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

      internal static async Task<Option<List<KeyValue>>> GetLookupsAsync<T, TReadLookupInterceptor>(
         this Option<ServiceMapping<T, TReadLookupInterceptor>> serviceMapping,
         Option<bool> useValueAsId,
         Option<CancellationToken> ctok)
         where T : class, IEntity
         where TReadLookupInterceptor : IReadLookupInterceptor
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
                     Option<List<KeyValue>> result;
                     if (x.Args.IsView)
                        result = await x.Repository.GetLookupsAsync(
                           x.Args.IdProperty,
                           x.Args.ValueProperty,
                           x.UseValueAsId,
                           x.Args.Predicate,
                           None.Value,
                           true,
                           ctok);
                     else
                        result = await x.Repository.GetLookupsAsync(
                           x.Args.IdProperty,
                           x.Args.ValueProperty,
                           x.UseValueAsId,
                           x.Args.Predicate,
                           x.Args.Include,
                           true,
                           ctok);

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

      internal static Option<IPaged<TDto>> GetPaged<T, TDto, TReadPagedInterceptor>(
         this Option<ServiceMapping<T, TReadPagedInterceptor>> serviceMapping,
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadPagedInterceptor : IReadPagedInterceptor
      {
         try
         {
            return serviceMapping.GetReadPagedParametersMapping(pageNo, pageSize, sorts, keyword)
               .MapFlatten(
                  x =>
                  {
                     AsyncHelper.RunSync(() => x.Args.PagedValidation?.Invoke(x.Repository));
                     Expression<Func<T, TDto>> toDtoExpr = y => y.ToDto<TDto>().ReduceOrDefault();
                     return x.Repository.GetPaged<TDto>(
                        toDtoExpr,
                        x.Args.SearchFields,
                        x.PageNo,
                        x.PageSize,
                        x.Sorts,
                        x.Keyword,
                        x.Args.DefaultPageIndexFrom,
                        x.Args.Predicate,
                        x.Args.IsView ? null : x.Args.Include,
                        true);
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

      internal static async Task<Option<IPaged<TDto>>> GetPagedAsync<T, TDto, TReadPagedInterceptor>(
         this Option<ServiceMapping<T, TReadPagedInterceptor>> serviceMapping,
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadPagedInterceptor : IReadPagedInterceptor
      {
         try
         {
            return await serviceMapping.GetAsyncReadPagedParametersMapping(pageNo, pageSize, sorts, keyword, ctok)
               .MapFlattenAsync(
                  async x =>
                  {
                     if (x.Args.PagedValidation != null) await x.Args.PagedValidation.Invoke(x.Repository);
                     Expression<Func<T, TDto>> toDtoExpr = y => y.ToDto<TDto>().ReduceOrDefault();
                     return await x.Repository.GetPagedAsync<TDto>(
                        toDtoExpr,
                        x.Args.SearchFields,
                        x.PageNo,
                        x.PageSize,
                        x.Sorts,
                        x.Keyword,
                        x.Args.DefaultPageIndexFrom,
                        x.Args.Predicate,
                        x.Args.IsView ? null : x.Args.Include,
                        true,
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

      internal static
         Option<(BaseUnitOfWork Uow, BaseRepository<T> Repository, ReadLookupInterceptorArgs<T> Args, bool UseValueAsId)>
         GetReadLookupParametersMapping<T, TReadLookupInterceptor>(
            this Option<ServiceMapping<T, TReadLookupInterceptor>> serviceMapping,
            Option<bool> useValueAsId)
         where T : class, IEntity
         where TReadLookupInterceptor : IReadLookupInterceptor
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
         Option<(BaseUnitOfWork Uow, BaseRepository<T> Repository, ReadOneInterceptorArgs<T> Args, TDto Criteria)>
         GetReadOneParametersMapping<T, TDto, TReadOneInterceptor>(
            this Option<ServiceMapping<T, TReadOneInterceptor>> serviceMapping,
            Option<TDto> criteria)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadOneInterceptor : IReadOneInterceptor
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
         Option<(BaseUnitOfWork Uow, BaseRepository<T> Repository, ReadPagedInterceptorArgs<T> Args, int PageNo, int PageSize,
            string[] Sorts, string Keyword)> GetReadPagedParametersMapping<T, TReadPagedInterceptor>(
            this Option<ServiceMapping<T, TReadPagedInterceptor>> serviceMapping,
            Option<int> pageNo,
            Option<int> pageSize,
            Option<string>[] sorts,
            Option<string> keyword)
         where T : class, IEntity
         where TReadPagedInterceptor : IReadPagedInterceptor
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

      internal static Option<(BaseUnitOfWork Uow, BaseRepository<T> Repository, SaveInterceptorArgs<T, TDto> Args, TDto Dto)>
         GetUpdateParametersMapping<T, TDto, TUpdateInterceptor>(
            this Option<ServiceMapping<T, TUpdateInterceptor>> serviceMapping,
            Option<TDto> dto)
         where T : class, IEntity
         where TDto : class, IDto
         where TUpdateInterceptor : IUpdateInterceptor
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

      internal static Option<Dictionary<string, object>> UpdateEntity<T, TDto, TUpdateInterceptor>(
         this Option<ServiceMapping<T, TUpdateInterceptor>> serviceMapping,
         Option<TDto> dto)
         where T : class, IEntity
         where TDto : class, IDto
         where TUpdateInterceptor : IUpdateInterceptor
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
                     var result = x.Repository.Update(entity, x.Args.MessageIfExistFunc);
                     AsyncHelper.RunSync(() => x.Args.PreSave?.Invoke(entity, x.Repository));
                     if (x.Args.PendingSave) return result;
                     x.Uow.SaveChanges();
                     return result;
                  });
         }
         catch (ArgumentNullException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (ValidationException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (KeyNotFoundException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PreValidationException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PostValidationException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PostMapException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PreSaveException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (ExistingEntityException<T> ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
#if NETSTANDARD2_0
         catch (EfCoreDbUpdateConcurrencyException ex)
         {
            var message = ex.HumanizeUpdateError();
            // ReSharper disable once SuspiciousTypeConversion.Global
            return Fail<Dictionary<string, object>>.Throw(
               new EfCoreDbUpdateConcurrencyException(message, ex.Entries));
         }
#endif
#if NET471
         catch (EfDbUpdateConcurrencyException ex)
         {
            var message = ex.HumanizeUpdateError();
            return Fail<Dictionary<string, object>>.Throw(new EfDbUpdateConcurrencyException(message, ex));
         }
#endif
#if NETSTANDARD2_0
         catch (EfCoreDbUpdateException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
#endif
#if NET471
         catch (EfDbUpdateException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
#endif
         catch (Exception ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
      }

      internal static async Task<Option<Dictionary<string, object>>> UpdateEntityAsync<T, TDto, TUpdateInterceptor>(
         this Option<ServiceMapping<T, TUpdateInterceptor>> serviceMapping,
         Option<TDto> dto,
         Option<CancellationToken> ctok)
         where T : class, IEntity
         where TDto : class, IDto
         where TUpdateInterceptor : IUpdateInterceptor
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
                     var result = await x.Repository.UpdateAsync(entity, x.Args.MessageIfExistFunc, ctok);
                     if (x.Args.PreSave != null) await x.Args.PreSave.Invoke(entity, x.Repository);
                     if (x.Args.PendingSave) return result;
                     await x.Uow.SaveChangesAsync(ctok);
                     return result;
                  });
         }
         catch (ArgumentNullException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (ValidationException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (KeyNotFoundException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PreValidationException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PostValidationException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PostMapException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (PreSaveException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
         catch (ExistingEntityException<T> ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
#if NETSTANDARD2_0
         catch (EfCoreDbUpdateConcurrencyException ex)
         {
            var message = await ex.HumanizeUpdateErrorAsync(ctok);
            // ReSharper disable once SuspiciousTypeConversion.Global
            return Fail<Dictionary<string, object>>.Throw(
               new EfCoreDbUpdateConcurrencyException(message, ex.Entries));
         }
#endif
#if NET471
         catch (EfDbUpdateConcurrencyException ex)
         {
            var message = await ex.HumanizeUpdateErrorAsync(ctok);
            return Fail<Dictionary<string, object>>.Throw(new EfDbUpdateConcurrencyException(message, ex));
         }
#endif
#if NETSTANDARD2_0
         catch (EfCoreDbUpdateException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
#endif
#if NET471
         catch (EfDbUpdateException ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
#endif
         catch (Exception ex)
         {
            return Fail<Dictionary<string, object>>.Throw(ex);
         }
      }

      #endregion
   }
}