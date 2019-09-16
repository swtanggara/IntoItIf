using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IntoItIf.AspNetMvc
{
   using System.Reflection;
   using global::Autofac;
   using global::Autofac.Integration.WebApi;

   public class WebApiApplication : System.Web.HttpApplication
   {
      protected void Application_Start()
      {
         AreaRegistration.RegisterAllAreas();

         var builder = new ContainerBuilder();
         var httpConfig = GlobalConfiguration.Configuration;
         builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
         builder.RegisterWebApiFilterProvider(httpConfig);
         builder.RegisterWebApiModelBinderProvider();
         var container = builder.Build();
         httpConfig.DependencyResolver = new AutofacWebApiDependencyResolver(container);
         GlobalConfiguration.Configure(WebApiConfig.Register);

         FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
         RouteConfig.RegisterRoutes(RouteTable.Routes);
         BundleConfig.RegisterBundles(BundleTable.Bundles);
      }
   }
}
