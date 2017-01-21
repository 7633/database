using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace database.Models
{
    [Table("customers")]
    public class Customer
    {
        public Customer()
        {
            Projects = new HashSet<Projects>();
        }
        [Column("id")]
        public int id { get; set; }
        [Column("name")]
        public string customerName { get; set; }
        [Column("surname")]
        public string customerSurname { get; set; }
        public ICollection<Projects> Projects { get; set; }
    }
}