namespace IntoItIf.Dsl.Mediator.Requests
{
   using System.Collections.Generic;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Core.Mediator;

   public sealed class ReadPagedRequest<T, TDto> : BaseEntityRequest<T, TDto>, IRequest<IPaged<TDto>>
      where T : class, IEntity where TDto : class, IDto
   {
      #region Constructors and Destructors

      public ReadPagedRequest(Option<int> pageNo, Option<int> pageSize, Option<string>[] sorts, Option<string> keyword)
      {
         PageNo = pageNo;
         PageSize = pageSize;
         Sorts = sorts;
         Keyword = keyword;
      }

      #endregion

      #region Properties

      internal Option<string> Keyword { get; }

      internal Option<int> PageNo { get; }
      internal Option<int> PageSize { get; }
      internal Option<string>[] Sorts { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return PageNo;
         yield return PageSize;
         yield return Sorts;
         yield return Keyword;
      }

      #endregion
   }
}