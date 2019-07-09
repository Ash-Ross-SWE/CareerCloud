using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCloud.Pocos
{
    [Table("System_Country_Codes")]
    public class SystemCountryCodePoco             
    {
        [Key]
        public String Code { get; set; }
        public String Name { get; set; }
        public string Login { get; set; }
        public object Password { get; set; }
    }
}
