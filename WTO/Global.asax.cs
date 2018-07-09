using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WTO.Controllers;
using WTO.Controllers.WTO;

namespace WTO
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            App_Start.FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            App_Start.BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {

            GlobalErrorController objGlobalError = new GlobalErrorController();
            LoginController objloginController = new LoginController();
            var excepMsg = "";
            var source = "";


            Exception exception = Server.GetLastError();
            excepMsg = exception.Message;
            source = exception.Source;
            objGlobalError.Error(excepMsg, source);
            Server.ClearError();

            //HttpContext.Current.Response.Redirect("~/Views/Shared/Error");
            //Debug.WriteLine("");
            //Response.Redirect("~/Views/_ViewStart.cshtml");
            Application_GlobalError();

        }

        protected void Application_GlobalError()
        {
            AreaRegistration.RegisterAllAreas();


            // RouteConfig.RegisterRoutes1(RouteTable.Routes);
            App_Start.FilterConfig.RegisterGlobalFilters1(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes1(RouteTable.Routes);
            Response.RedirectToRoute("404-PageNotFound");
        }

        protected void Application_End()
        {

        }
    }
}
