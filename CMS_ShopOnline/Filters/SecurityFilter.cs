using System.Web.Mvc;
using WebMatrix.WebData;

namespace CMS_ShopOnline.App_Start
{
    public class SecurityFilter : ActionFilterAttribute
    {
        private bool _authenticate;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                CMS_ShopOnline.Helpers.Helper.Request();
            }
            _authenticate = (filterContext.ActionDescriptor.GetCustomAttributes(typeof(OverrideAuthenticationAttribute), true).Length == 0);
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (skipAuthorization)
            {
                return;
            }
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DBConnection", "NhanVien", "Id", "TenTaiKhoan", true);
            }
            base.OnActionExecuting(filterContext);
            //string sControlerName = filterContext.RouteData.Values["Controller"] != null ? filterContext.RouteData.Values["Controller"].ToString().ToLower() : "";
            //string sActionName = filterContext.RouteData.Values["Action"] != null ? filterContext.RouteData.Values["Action"].ToString().ToLower() : "";
            //if (sActionName != "login".ToLower() && sActionName != "logoff")
            //{
            //    if (AccessRight(sActionName, sControlerName))
            //    {
            //        filterContext.Controller.ViewBag.IdMenuItem = 1;
            //    }
            //    else
            //    {
            //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" }, { "area", "" } });
            //    }
            //}

        }
        //public static bool AccessRight(string ActionName, string ControlerName)
        //{
        //    string sControlerName = ControlerName != null ? ControlerName.ToLower() : "";
        //    string sActionName = ActionName != null ? ActionName.ToLower() : "";

        //    if (sControlerName == "" || sActionName == "")
        //        return false;
        //    if (WebSecurity.IsAuthenticated)
        //    {
        //        ControllerRepository x = new ControllerRepository(new DB());
        //        var list = x.GetByName(sActionName, sControlerName);
        //        if (list == null)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //        return false;

        //}
    }
}