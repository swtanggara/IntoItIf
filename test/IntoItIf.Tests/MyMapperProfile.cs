namespace IntoItIf.Tests
{
   using System;
   using Core.Domain;
   using Core.Domain.Options;

   public class MyMapperProfile : IMapperProfile
   {
      public Option<(Type Source, Type Destination)>[] GetBinds()
      {
         return new Option<(Type Source, Type Destination)>[]
         {
             (typeof(MyEntity), typeof(MyDto)),
             (typeof(MyDto), typeof(MyEntity))
         };
      }
   }
}
