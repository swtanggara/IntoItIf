namespace IntoItIf.Tests.Preparation
{
   using System;
   using Base.Domain.Options;
   using Base.Mappers;

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
