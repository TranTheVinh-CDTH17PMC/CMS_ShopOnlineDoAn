using CMS_Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_Database.Entities;
using System.Data.Entity;

namespace CMS_Database.Repositories
{
    public class NhanVienRepository : GenericRepository<NhanVien>,INhanVien
    {
        public NhanVien GetbyId(int Id)
        {
            return _db.NhanVien.SingleOrDefault(m => m.Id == Id && m.IsDelete != true);
        }

        public NhanVien GetByUserName(string name)
        {
            return _db.NhanVien.SingleOrDefault(m => m.TenTaiKhoan == name && m.IsDelete!=true);
        }
    }
}
