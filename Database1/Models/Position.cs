using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace database.Models
{
    [Table("position")]
    public class Position
    {
        [Column("id")]
        public int id { get; set; }
        [Column("positionName")]
        public string PositionName { get; set; }

    }
}