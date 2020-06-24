using CMS_ShopOnline.Areas.Crm.Models;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.Crm
{
    public class CrmAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Crm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Crm_default",
                "Crm/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            CreateMap();
        }
        public void CreateMap()
        {
            AutoMapper.Mapper.CreateMap<CMS_Database.Entities.NhaCungCap, NhaCungCapViewModel>();
            AutoMapper.Mapper.CreateMap<NhaCungCapViewModel, CMS_Database.Entities.NhaCungCap>();
        }
    }
}