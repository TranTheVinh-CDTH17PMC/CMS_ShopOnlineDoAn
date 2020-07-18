using CMS_Database.Entities;
using CMS_Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Interfaces
{
    public class ICTPhieuPhapRepository : GenericRepository<CTPhieuNhap>, ICTPhieuNhap
    {
        public IQueryable<CTPhieuNhap> GetById(int id)
        {
            return _db.CTPhieuNhap.Where(x => x.IdPhieuNhap == id);
        }
    }
}
