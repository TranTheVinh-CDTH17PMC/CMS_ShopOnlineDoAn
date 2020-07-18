using CMS_Database.Entities;
using CMS_Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Repositories
{
    public class CTHoaDonRepository:GenericRepository<CTHoaDon>,ICTHoaDon
    {
        public IQueryable<CTHoaDon> GetById(int id)
        {
            return _db.CTHoaDon.Where(x => x.IdHoaDon == id);
        }
    }
}
