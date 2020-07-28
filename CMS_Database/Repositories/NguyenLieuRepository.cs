using CMS_Database.Entities;
using CMS_Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Repositories
{
    public class NguyenLieuRepository : GenericRepository<NguyenLieu>, INguyenLieu
    {
        public int? GetSLbyId(int id)
        {
            var soluong = _db.NguyenLieu.Where(x => x.IsDelete != null && x.Id == id).Select(x => x.SoLuongKho).FirstOrDefault();
            int? soluongreal = soluong;
            return soluongreal;
        }
    }
}
