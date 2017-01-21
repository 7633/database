using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace database.Models
{
    [Table("participation")]
    public class Participation
    {
        [Key]
        [Column("students_id")]
        public int studentsId { get; set; }
        [Column("projects_id")]
        public int projectsId { get; set; }
        [Column("role")]
        public string role { get; set; }
      
    }
}