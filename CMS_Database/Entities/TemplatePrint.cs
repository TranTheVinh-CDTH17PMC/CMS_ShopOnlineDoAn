using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Entities
{
    [Table("TemplatePrint")]
    public partial class TemplatePrint
    {
       public  int? Id { set; get; }
        public string Content { set; get; }
    }
}
