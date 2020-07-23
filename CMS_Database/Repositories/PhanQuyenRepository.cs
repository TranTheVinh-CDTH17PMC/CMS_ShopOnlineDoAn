using CMS_Database.Entities;
using CMS_Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Repositories
{
    public class PhanQuyenRepository : GenericRepository<PhanQuyen>, IPhanQuyen
    {
        public PhanQuyen GetByName(int? sControlerName,int? IdRole)
        {
            return _db.PhanQuyen.FirstOrDefault(x => x.IdControllerName == sControlerName && x.IdRole== IdRole && x.IsDelete!=true);
        }
    }
}
