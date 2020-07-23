using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Entities
{
    [Table("ListController")]
    public partial class ListController
    {
        public int? Id { get; set; }
        public string ControllerName { get; set; }
        public bool? IsDelete { get; set; }
    }
}
