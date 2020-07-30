using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Entities
{
    [Table("DoiDiem")]
    public partial class DoiDiem
    {
       public int? Id { get; set; }
       public double? Tienhang { get; set; }
       public int? Diemhang { get; set; }
       public double? Tiendoi { get; set; }
       public int? Diemdoi { get; set; }
       public bool? IsDelete { get; set; }
    }
}
