using CMS_ShopOnline.Areas.Administration.Models;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Administration_default",
                "Administration/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            CreateMap();
        }
        public void CreateMap()
        {
            AutoMapper.Mapper.CreateMap<CMS_Database.Entities.DoiDiem, DoiDiemViewModel>();
            AutoMapper.Mapper.CreateMap<DoiDiemViewModel, CMS_Database.Entities.DoiDiem>();

        }
    }
}