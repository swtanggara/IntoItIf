namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System.Collections.Generic;
   using Core.Domain.Entities;

   public class MyDto : BaseDto<MyDto, MyDtoFluentValidator>
   {
      public int Id { get; set; }
      public string Name { get; set; }
   }
}