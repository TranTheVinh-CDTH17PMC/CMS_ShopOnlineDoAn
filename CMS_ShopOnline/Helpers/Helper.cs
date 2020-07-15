using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WebMatrix.WebData;

namespace CMS_ShopOnline.Helpers
{
    public class Helper
    {
        public static List<RequestInfo> ListRequest
        {
            set // đọc giá trị truyền vào
            {
                HttpContext.Current.Application["ListRequest"] = value;
            }
            get // trả giá trị sao khi đọc
            {
                var ListRequest = HttpContext.Current.Application["ListRequest"] as List<RequestInfo>; //as để covert Application sang List<RequestInfo>
                return ListRequest;
            }
        }
        public static void Request()
        {
            if (ListRequest == null)
                ListRequest = new List<RequestInfo>();
            var ip = HttpContext.Current.Request.UserHostAddress;
            var requestInfo = ListRequest.Where(x=>x.IP == ip).FirstOrDefault();
            if (requestInfo != null && WebSecurity.IsAuthenticated && requestInfo.User != null && requestInfo.User.TenTaiKhoan != WebSecurity.CurrentUserName.ToLower())
            {
                requestInfo = null;
            }
            if (requestInfo == null)
            {
                requestInfo = new RequestInfo();
                requestInfo.IP = ip;         
                ListRequest.Add(requestInfo);
            }
            if (WebSecurity.IsAuthenticated && requestInfo.User == null)
            {
                INhanVien _nhanvien = new NhanVienRepository();
                NhanVien nhanvien = _nhanvien.GetByUserName(WebSecurity.CurrentUserName);
                requestInfo.User = nhanvien;
            }

        }
        public static CMS_Database.Entities.NhanVien CurrentUser
        {
            get
            {
                var ListRequest = HttpContext.Current.Application["ListRequest"] as List<RequestInfo>;
                if (ListRequest != null)
                {
                    var user1 = ListRequest.Where(item => item.User != null)
                        .Select(item => item.User);
                    var count = user1.Count();
                    if (user1 != null && user1.Count() > 0)
                    {
                        return user1.ElementAt(user1.Count() - 1); // trừ 1 vì list bắt đầu là số 0
                    }
                }

                return null;
            }
            set
            {
                var ListRequest = HttpContext.Current.Application["ListRequest"] as List<RequestInfo>;
                if (ListRequest != null)
                {
                    var r = ListRequest.Where(item => item.User != null && item.User.TenTaiKhoan == WebSecurity.CurrentUserName).FirstOrDefault();
                    if (r != null)
                        r.User = value;
                }
            }
        }
        public static string ChuyenThanhKhongDau(string s)
        {
            if (string.IsNullOrEmpty(s) == true)
                return "";

            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
        }
        public static bool IsPreparation()
        {
            if(ChuyenThanhKhongDau(CurrentUser.LoaiNV.TenLoai) == ChuyenThanhKhongDau("Pha Chế"))
            {
                return true;
            }
            return false;
        }

    }
}