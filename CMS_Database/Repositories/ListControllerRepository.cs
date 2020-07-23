using CMS_Database.Entities;
using CMS_Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Repositories
{
    public class ListControllerRepository : GenericRepository<ListController>, IListController
    {
        public ListController GetIdByName(string name)
        {
            return _db.ListController.FirstOrDefault(x => x.ControllerName == name && x.IsDelete != true);
        }
    }
}
