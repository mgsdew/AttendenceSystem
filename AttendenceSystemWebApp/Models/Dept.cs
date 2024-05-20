using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using AttendenceSystemWebApp.Models;
namespace AttendenceSystemWebApp
{
    [Table("Dept")]
    public class Dept:BaseEntity
    {
        [StringLength(3)]
        public string DeptId { get; set; }

        [StringLength(25)]
        [Display(Name ="Department Name")]
        public string DeptNm { get; set; }
    }
}
