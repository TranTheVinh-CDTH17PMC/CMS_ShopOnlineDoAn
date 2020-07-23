using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Entities
{
    [Table("PhanQuyen")]
    public partial class PhanQuyen
    {
        public int? Id { get; set; }
        public int? IdControllerName { get; set; }
        public int? IdRole { get; set; }
        public bool IsDelete { get; set; }
    }
}
