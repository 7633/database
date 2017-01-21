using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace database.Models
{
    [Table("projects")]
    public class Projects
    {
        public Projects()
        {
            Customers = new HashSet<Customer>();
            Departments = new HashSet<Departments>();
        }
        [Column("id")]
        public int id { get; set; }
        [Column("name")]
        public string ProjectName { get; set; }
        [Column("beginDate")]
        public DateTime? beginDate { get; set; }
        [Column("endDate")]
        public DateTime? endDate { get; set; }
        [Column("realEndDate")]
        public DateTime? realEndDate { get; set; }
        [Column("cost")]
        public int? cost { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<Departments> Departments { get; set; }
        
    }
}