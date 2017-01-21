using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace database.Models
{
    [Table("students")]
    public class Student
    {
        [Column("id")]
        public int id { get; set; }
        [Column("name")]
        public string name { get; set; }
        [Column("surname")]
        public string surname { get; set; }
        [Column("departments_id")]
        public int departmentsId { get; set; }
        [Column("position_id")]
        public int positionId { get; set; }
    }
}