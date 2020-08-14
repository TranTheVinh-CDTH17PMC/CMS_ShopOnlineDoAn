using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using WebMatrix.WebData;

namespace CMS_ShopOnline.App_Start
{
    
    public class SecurityFilter : ActionFilterAttribute
    {
        private bool _authenticate;
        private readonly INhanVien _NhanVien = new NhanVienRepository();
        private readonly IPhanQuyen _PhanQuyen = new PhanQuyenRepository();
        private readonly IListController _ListController = new ListControllerRepository();
        private readonly ILoaiNV _LoaiNV = new LoaiNVRepository();
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
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "User" }, { "action", "NotPage" }, { "area", "Administration" } });
                }
            }
            //Lấy danh sách các controller hiện có
            Assembly asm = Assembly.GetExecutingAssembly();

            var controllerTypes = from t in asm.GetExportedTypes()
                                  where typeof(IController).IsAssignableFrom(t)
                                  select t;
            //Nếu chưa có gì trong bảng thì thêm controller vô
            if(_ListController.SelectAll().Count() == 0)
            {
                foreach (var item in controllerTypes)
                {
                    var ctl = new ListController();
                    ctl.ControllerName = item.Name.Replace("Controller", "");
                    ctl.IsDelete = false;
                    _ListController.Insert(ctl);
                    _ListController.Save();
                }
                var getlistctl = _ListController.SelectAll().Where(x => x.IsDelete != true);
                var getlistrole = _LoaiNV.SelectAll().Where(x => x.IsDelete != true);
                foreach (var itemrole in getlistrole)
                {
                    foreach (var itemctl in getlistctl)
                    {
                        var pq = new PhanQuyen();
                        pq.IdControllerName = itemctl.Id;
                        pq.IdRole = itemrole.Id;
                        pq.IsDelete = false;
                        _PhanQuyen.Insert(pq);
                        _PhanQuyen.Save();
                    }
                }
            }
            //nếu thay thêm/xóa controlle thì cập nhật lại
            if(controllerTypes.Count() != _ListController.SelectAll().Count())
            {
                _ListController.DeleteAll();
                _ListController.Save();
                _PhanQuyen.DeleteAll();
                _PhanQuyen.Save();
                //-----
                foreach (var item in controllerTypes)
                {
                    var ctl = new ListController();
                    ctl.ControllerName = item.Name.Replace("Controller", "");
                    ctl.IsDelete = false;
                    _ListController.Insert(ctl);
                    _ListController.Save();
                }
                var getlistctl = _ListController.SelectAll().Where(x => x.IsDelete != true);
                var getlistrole = _LoaiNV.SelectAll().Where(x => x.IsDelete != true);
                foreach (var itemrole in getlistrole)
                {
                    foreach (var itemctl in getlistctl)
                    {
                        var pq = new PhanQuyen();
                        pq.IdControllerName = itemctl.Id;
                        pq.IdRole = itemrole.Id;
                        pq.IsDelete = false;
                        _PhanQuyen.Insert(pq);
                        _PhanQuyen.Save();
                    }
                }
            }
            DeleteKMQH();
        }
        public static void DeleteKMQH()
        {
            IKhuyenMai KhuyenMai = new KhuyenMaiRepository();
            var daynow = DateTime.Now.Date;
            var abc = KhuyenMai.SelectAll().Where(x => x.IsDelete != true);
            foreach (var item in abc)
            {
                if (daynow > item.NgayKT.Date)
                {
                    item.IsDelete = true;
                    KhuyenMai.Update(item);
                    KhuyenMai.Save();
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