using CMS_Database.Entities;
using CMS_Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Repositories
{
    public class ThanhPhamRepository : GenericRepository<ThanhPham>, IThanhPham
    {
        public void Delele(int id)
        {
        }
        public ThanhPham GetbyId(int Id)
        {
            return _db.ThanhPham.SingleOrDefault(m => m.Id == Id);
        }
    }
}
