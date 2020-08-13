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
    public static class Helper
    {
        public static DateTime GetFistDayInMonth(int year, int month)
        {
            DateTime aDateTime = new DateTime(year, month, 1);

            return aDateTime;
        }

        // Trả về ngày cuối cùng của tháng.
        public static DateTime GetLastDayInMonth(int year, int month)
        {
            DateTime aDateTime = new DateTime(year, month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);

            return retDateTime;
        }
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

            var requestInfo = ListRequest.Where(x => x.IP == ip).FirstOrDefault();
            if (requestInfo == null && WebSecurity.CurrentUserName != null && WebSecurity.CurrentUserName != "")
            {
                requestInfo = ListRequest.Where(x => x.IP == ip && x.User.TenTaiKhoan == WebSecurity.CurrentUserName).FirstOrDefault();
            }
            if (requestInfo != null && WebSecurity.IsAuthenticated && requestInfo.User != null && requestInfo.User.TenTaiKhoan != WebSecurity.CurrentUserName)
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
                    INhanVien _nhanvien = new NhanVienRepository();
                    var user1 = _nhanvien.GetByUserName(WebSecurity.CurrentUserName);
                    if (user1 != null)
                    {
                        return user1;
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
            if(ChuyenThanhKhongDau(CurrentUser.LoaiNV.TenCode) == ChuyenThanhKhongDau("PC"))
            {
                return true;
            }
            return false;
        }
        public static bool IsGD()
        {
            if (ChuyenThanhKhongDau(CurrentUser.LoaiNV.TenCode) == ChuyenThanhKhongDau("GD"))
            {
                return true;
            }
            return false;
        }
        public static bool Khuyenmai()
        {
            IDoiDiem doidiem = new DoiDiemRepository();
            var _dd = doidiem.SelectAll().Where(x => x.IsDelete != true).Count();
            if(_dd>0)
            {
                return false;
            }
            return true;
        }
        public static bool IsManager()
        {
            if (ChuyenThanhKhongDau(CurrentUser.LoaiNV.TenCode) == ChuyenThanhKhongDau("GD"))
            {
                return true;
            }
            if(ChuyenThanhKhongDau(CurrentUser.LoaiNV.TenCode) == ChuyenThanhKhongDau("QL"))
            {
                 return true;
            }
            return false;
        }
        public static string ToCurrencyStr(this double? value, string currency)
        {
            if(value != 0 && value.ToString() != "")
            {
                string semiRes = value.ToString();
                var lastThree = semiRes.Substring(semiRes.Length - 3, 3);
                List<string> resulatArray = new List<string>();
                resulatArray.Add(lastThree);
                semiRes = semiRes.Substring(0, semiRes.Length - 3);
                for (int i = 3; i <= semiRes.Length + 3; i = i + 3)
                {
                    var start = semiRes.Length - i;
                    var len = 3;
                    if (start < 0)
                    {
                        len = 3 + start;
                        start = 0;
                    }
                    var nextTwo = semiRes.Substring(start, len);
                    resulatArray.Insert(0, nextTwo);
                }
                var result = string.Join(".", resulatArray);
                if (result.StartsWith("."))
                {
                    result = result.Substring(1);
                }
                return result;
            }
            else
            {
                value = 0;
            }
            return value.ToString();
        }
        public static bool CheckKhuyenMai()
        {
            var daynow = DateTime.Now.Date;
            IKhuyenMai km = new KhuyenMaiRepository();
            var model = km.SelectAll().Where(x => x.IsDelete != true);
            foreach(var item in model)
            {
                if(daynow >= item.NgayBD.Date  && daynow <= item.NgayKT.Date)
                {
                    return true;
                }
            }
            return false;
        }
    }
}