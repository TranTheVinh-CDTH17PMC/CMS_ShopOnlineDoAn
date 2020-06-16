using CMS_Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Interfaces
{
    public interface INhanVien
    {
        NhanVien GetbyId(int Id);
        void Update(NhanVien nhanvien);
        void Save();
        NhanVien GetByUserName(string name);
    }
}
