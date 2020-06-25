using CMS_ShopOnline.Areas.CMS_Staff.Models;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.CMS_Staff
{
    public class StaffAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CMS_Staff";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CMS_Staff_default",
                "CMS_Staff/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            CreateMap();
        }
        public void CreateMap()
        {
            AutoMapper.Mapper.CreateMap<CMS_Database.Entities.NhanVien, NhanVienViewModel>();
            AutoMapper.Mapper.CreateMap<NhanVienViewModel, CMS_Database.Entities.NhanVien>();
        }
    }
}