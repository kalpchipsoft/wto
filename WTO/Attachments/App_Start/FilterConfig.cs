using System.Web;
using System.Web.Mvc;

namespace WTO.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterGlobalFilters__WithGlobalError(GlobalFilterCollection filters)
        {
            //GlobalFilterCollection filters = new GlobalFilterCollection();
            filters.Add(new HandleErrorAttribute());
        }
    }
}