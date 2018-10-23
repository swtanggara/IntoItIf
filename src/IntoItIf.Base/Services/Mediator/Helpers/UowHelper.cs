namespace IntoItIf.Base.Services.Mediator.Helpers
{
   using Domain.Entities;
   using Domain.Options;
   using Entities.Interceptors;
   using Repositories;
   using UnitOfWork;

   public static class UowHelper
   {
      #region Public Methods and Operators

      public static Create<T, TDto, TCreateInterceptor, TRepository> Create<T, TDto, TCreateInterceptor, TRepository>(this Option<IUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TCreateInterceptor : ICreateInterceptor
         where TRepository : class, IRepository<T>
      {
         return new Create<T, TDto, TCreateInterceptor, TRepository>(uow);
      }

      public static Crud<T, TDto, TCrudInterceptor, TRepository> Crud<T, TDto, TCrudInterceptor, TRepository>(this Option<IUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TCrudInterceptor : ICrudInterceptor
         where TRepository : class, IRepository<T>
      {
         return new Crud<T, TDto, TCrudInterceptor, TRepository>(uow);
      }

      public static Delete<T, TDto, TDeleteInterceptor, TRepository> Delete<T, TDto, TDeleteInterceptor, TRepository>(this Option<IUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TDeleteInterceptor : IDeleteInterceptor
         where TRepository : class, IRepository<T>
      {
         return new Delete<T, TDto, TDeleteInterceptor, TRepository>(uow);
      }

      public static Read<T, TDto, TReadInterceptor, TRepository> Read<T, TDto, TReadInterceptor, TRepository>(this Option<IUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadInterceptor : IReadInterceptor
         where TRepository : class, IRepository<T>
      {
         return new Read<T, TDto, TReadInterceptor, TRepository>(uow);
      }

      public static ReadLookup<T, TDto, TReadLookupInterceptor, TRepository> ReadLookup<T, TDto, TReadLookupInterceptor, TRepository>(
         this Option<IUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadLookupInterceptor : IReadLookupInterceptor
         where TRepository : class, IRepository<T>
      {
         return new ReadLookup<T, TDto, TReadLookupInterceptor, TRepository>(uow);
      }

      public static ReadOne<T, TDto, TReadOneInterceptor, TRepository> ReadOne<T, TDto, TReadOneInterceptor, TRepository>(
         this Option<IUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadOneInterceptor : IReadOneInterceptor
         where TRepository : class, IRepository<T>
      {
         return new ReadOne<T, TDto, TReadOneInterceptor, TRepository>(uow);
      }

      public static ReadPaged<T, TDto, TReadPagedInterceptor, TRepository> ReadPaged<T, TDto, TReadPagedInterceptor, TRepository>(
         this Option<IUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TReadPagedInterceptor : IReadPagedInterceptor
         where TRepository : class, IRepository<T>
      {
         return new ReadPaged<T, TDto, TReadPagedInterceptor, TRepository>(uow);
      }

      public static Update<T, TDto, TUpdateInterceptor, TRepository> Update<T, TDto, TUpdateInterceptor, TRepository>(this Option<IUow> uow)
         where T : class, IEntity
         where TDto : class, IDto
         where TUpdateInterceptor : IUpdateInterceptor
         where TRepository : class, IRepository<T>
      {
         return new Update<T, TDto, TUpdateInterceptor, TRepository>(uow);
      }

      #endregion
   }
}