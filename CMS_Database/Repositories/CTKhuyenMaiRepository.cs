using CMS_Database.Entities;
using CMS_Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Repositories
{
    public class CTKhuyenMaiRepository : GenericRepository<CTKhuyenmai>, ICTKhuyenMai
    {
        public IQueryable<CTKhuyenmai> GetById(int id)
        {
            return _db.CTKhuyenmai.Where(x => x.IdKhuyenMai == id);
        }
    }
}
