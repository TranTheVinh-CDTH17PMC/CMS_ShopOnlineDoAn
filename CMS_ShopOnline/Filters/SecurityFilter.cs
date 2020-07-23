using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using System.Web.Mvc;
using System.Web.Routing;
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
            string sControlerName = filterContext.RouteData.Values["Controller"] != null ? filterContext.RouteData.Values["Controller"].ToString().ToLower() : "";
            string sActionName = filterContext.RouteData.Values["Action"] != null ? filterContext.RouteData.Values["Action"].ToString().ToLower() : "";
            if (sActionName != "login".ToLower() && sActionName != "logoff" && sControlerName != "task" && sControlerName != "user")
            {
                if (!AccessRight(sControlerName))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Setting" }, { "action", "NotPage" }, { "area", "Administration" } });
                }
            }

        }
        public static bool AccessRight(string ControlerName)
        {
            string sControlerName = ControlerName != null ? ControlerName.ToLower() : "";
            if (sControlerName == "")
                return false;
            if (WebSecurity.IsAuthenticated)
            {
                var IdRole = Helpers.Helper.CurrentUser.IdLoaiNV;
                IListController _lc = new ListControllerRepository();
                IPhanQuyen _pq = new PhanQuyenRepository();
                var ListController = _lc.GetIdByName(sControlerName);
                var list = _pq.GetByName(ListController.Id,IdRole);
                if (list == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
                return false;

        }
    }
}