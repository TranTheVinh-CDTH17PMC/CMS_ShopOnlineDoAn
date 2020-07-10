using CMS_Database.Entities;
using CMS_Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Interfaces
{
    public class ICTPhieuXuatRepository : GenericRepository<CTPhieuXuat>, ICTPhieuXuat
    {
        public IQueryable<CTPhieuXuat> GetById(int id)
        {
            return _db.CTPhieuXuat.Where(x => x.IdPhieuXuat == id);
        }
    }
}
