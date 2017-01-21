using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace database.Models
{
    [Table("departments")]
    public class Departments
    {
        public Departments()
        {
            Projects = new HashSet<Projects>();
        }
        [Column("id")]
        public int id { get; set; }
        [Column("name")]
        public string departmentName { get; set; }
        public ICollection<Projects> Projects { get; set; }

    }
}