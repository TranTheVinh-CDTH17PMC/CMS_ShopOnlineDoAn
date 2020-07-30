using CMS_Database.Entities;
using CMS_Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Repositories
{
   public class KhachHangRepository : GenericRepository<KhachHang>, IKhachHang
    {
       public void Delele(int id)
       {
       }

        public KhachHang GetNameById(int? id)
        {
            var tenkh = _db.KhachHang.SingleOrDefault(x => x.IsDelete != true && x.Id == id);
            return tenkh;
        }

    }
}
