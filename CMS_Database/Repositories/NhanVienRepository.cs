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
    public class NhanVienRepository : INhanVien
    {
        private DBConnection db;
        public NhanVienRepository()
        {
            db = new DBConnection();
        }
        public NhanVien GetbyId(int Id)
        {
            return db.NhanVien.SingleOrDefault(m => m.Id == Id && m.IsDelete != true);
        }

        public NhanVien GetByUserName(string name)
        {
            return db.NhanVien.SingleOrDefault(m => m.TenTaiKhoan == name);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(NhanVien nhanvien)
        {
            if (nhanvien.Id == 0)
            {
                db.NhanVien.Add(nhanvien);
            }
            else
            {
                db.Entry(nhanvien).State = EntityState.Modified;
            }

            db.SaveChanges();
        }
    }
}
