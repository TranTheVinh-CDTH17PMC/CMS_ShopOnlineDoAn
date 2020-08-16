using CMS_Database.Entities;
using CMS_Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CMS_Database.Repositories
{
    public class ThanhPhamRepository : GenericRepository<ThanhPham>, IThanhPham
    {
        public static string ChuyenThanhKhongDau(string s)
        {
            if (string.IsNullOrEmpty(s) == true)
                return "";

            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
        }
        public void Delele(int id)
        {
        }
        public ThanhPham GetbyId(int Id)
        {
            return _db.ThanhPham.SingleOrDefault(m => m.Id == Id);
        }

        public ThanhPham GetbyName(string name)
        {
            return _db.ThanhPham.FirstOrDefault(m => m.Ten == name);
        }
    }
}
