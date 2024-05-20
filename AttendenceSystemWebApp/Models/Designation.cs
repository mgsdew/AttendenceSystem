
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using AttendenceSystemWebApp.Models;


namespace AttendenceSystemWebApp
{
    [Table("Designation")]
    public class Designation:BaseEntity
    {
        [Key]
        [StringLength(3)]
        public string DesgId { get; set; }

        [StringLength(30)]
        public string Desg { get; set; }

        [StringLength(1)]
        public string Supervisor { get; set; }

        public byte? GrdFr { get; set; }

        public byte? GrdTo { get; set; }
    }
}
