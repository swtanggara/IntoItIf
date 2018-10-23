namespace IntoItIf.Ef.Service.Mediator.Helpers
{
   using Base.Domain.Entities;
   using Base.Domain.Options;
   using Base.Services.Entities.Interceptors;
   using UnitOfWork;

   public static class UowHelper
   {
      #region Public Methods and Operators

      public static Create<T, TDto, TCreateInterceptor> Create<T, TDto, TCreateInterceptor>(this Option<EfUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TCreateInterceptor : ICreateInterceptor
      {
         return new Create<T, TDto, TCreateInterceptor>(uow);
      }

      public static Crud<T, TDto, TCrudInterceptor> Crud<T, TDto, TCrudInterceptor>(this Option<EfUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TCrudInterceptor : ICrudInterceptor
      {
         return new Crud<T, TDto, TCrudInterceptor>(uow);
      }

      public static Delete<T, TDto, TDeleteInterceptor> Delete<T, TDto, TDeleteInterceptor>(this Option<EfUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TDeleteInterceptor : IDeleteInterceptor
      {
         return new Delete<T, TDto, TDeleteInterceptor>(uow);
      }

      public static Read<T, TDto, TReadInterceptor> Read<T, TDto, TReadInterceptor>(this Option<EfUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadInterceptor : IReadInterceptor
      {
         return new Read<T, TDto, TReadInterceptor>(uow);
      }

      public static ReadLookup<T, TDto, TReadLookupInterceptor> ReadLookup<T, TDto, TReadLookupInterceptor>(
         this Option<EfUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadLookupInterceptor : IReadLookupInterceptor
      {
         return new ReadLookup<T, TDto, TReadLookupInterceptor>(uow);
      }

      public static ReadOne<T, TDto, TReadOneInterceptor> ReadOne<T, TDto, TReadOneInterceptor>(
         this Option<EfUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadOneInterceptor : IReadOneInterceptor
      {
         return new ReadOne<T, TDto, TReadOneInterceptor>(uow);
      }

      public static ReadPaged<T, TDto, TReadPagedInterceptor> ReadPaged<T, TDto, TReadPagedInterceptor>(
         this Option<EfUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadPagedInterceptor : IReadPagedInterceptor
      {
         return new ReadPaged<T, TDto, TReadPagedInterceptor>(uow);
      }

      public static Update<T, TDto, TUpdateInterceptor> Update<T, TDto, TUpdateInterceptor>(this Option<EfUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TUpdateInterceptor : IUpdateInterceptor
      {
         return new Update<T, TDto, TUpdateInterceptor>(uow);
      }

      #endregion
   }
}