using CMS_Database.Entities;
using CMS_Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Interfaces
{
    public class CongViecRepository : GenericRepository<CongViec>, ICongViec
    {
        public IEnumerable<CongViec> GetAll()
        {
            return _db.CongViec.Where(x => x.IsDelete != true).OrderByDescending(x => x.NgayTao).Take(5);
        }

        public void InsertTask(CongViec obj)
        {
            _db.CongViec.Add(obj);
            _db.SaveChanges();
        }
    }
}
