namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System.Threading;
   using Core.Domain.Options;
   using Entities.Services;

   internal static class RequestHandlerHelper
   {
      #region Methods

      private static void Client()
      {
         Option<CancellationToken> ctok = CancellationToken.None;
         var dto = new MyDto();

         new EntityCreateService<MyEntity, MyDto, MyCreateInterceptor>().CreateEntityAsync(dto, ctok);
         new EntityDeleteService<MyEntity, MyDto, MyDeleteInterceptor>().DeleteEntityAsync(dto, ctok);
         new EntityReadLookupService<MyEntity, MyReadLookupInterceptor>().GetLookupsAsync(false, ctok);
         new EntityReadOneService<MyEntity, MyDto, MyReadOneInterceptor>().GetByPredicateAsync(dto, ctok);
         new EntityReadPagedService<MyEntity, MyDto, MyReadPagedInterceptor>().GetPagedAsync(
            1,
            1,
            null,
            "bla",
            ctok);

         Create<MyEntity, MyDto, MyCreateInterceptor>.Handle(dto, ctok);
         Delete<MyEntity, MyDto, MyDeleteInterceptor>.Handle(dto, ctok);
         ReadLookup<MyEntity, MyDto, MyReadLookupInterceptor>.Handle(false, ctok);
         ReadOne<MyEntity, MyDto, MyReadOneInterceptor>.Handle(dto, ctok);
         ReadPaged<MyEntity, MyDto, MyReadPagedInterceptor>.Handle(1, 1, null, "Bla", ctok);
         Update<MyEntity, MyDto, MyUpdateInterceptor>.Handle(dto, ctok);

         Read<MyEntity, MyDto, MyReadInterceptor>.Handle(false, ctok);
         Read<MyEntity, MyDto, MyReadInterceptor>.Handle(dto, ctok);
         Read<MyEntity, MyDto, MyReadInterceptor>.Handle(1, 1, null, "bla", ctok);

         Crud<MyEntity, MyDto, MyCrudInterceptor>.HandleCreate(dto, ctok);
         Crud<MyEntity, MyDto, MyCrudInterceptor>.HandleDelete(dto, ctok);
         Crud<MyEntity, MyDto, MyCrudInterceptor>.HandleReadLookup(false, ctok);
         Crud<MyEntity, MyDto, MyCrudInterceptor>.HandleReadOne(dto, ctok);
         Crud<MyEntity, MyDto, MyCrudInterceptor>.HandleReadPaged(1, 1, null, "Bla", ctok);
         Crud<MyEntity, MyDto, MyCrudInterceptor>.HandleUpdate(dto, ctok);
      }

      #endregion
   }
}