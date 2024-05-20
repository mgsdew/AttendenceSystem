using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AttendenceSystemWebApp.Models
{
    [Table("Entity")]
    public class Entity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EID { get; set; }

        [Required]
        [StringLength(50)]
        public string eName { get; set; }

        [StringLength(50)]
        public string EntityType { get; set; }

        [StringLength(50)]
        public string ParentEntity { get; set; }

        public int? ActiveDeactive { get; set; }

        [StringLength(20)]
        public string EntityCode { get; set; }

        [StringLength(50)]
        public string ContactPerson { get; set; }

        [StringLength(50)]
        public string ContactNo { get; set; }

        public int? ZoneID { get; set; }

        [StringLength(50)]
        public string PhoneNo { get; set; }

        [StringLength(50)]
        public string EmailAdd { get; set; }
    }
}