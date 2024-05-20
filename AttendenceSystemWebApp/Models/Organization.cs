namespace RangsAttendanceWebApp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Organization")]
    public partial class Organization
    {
        [Key]
        [StringLength(3)]
        public string OrgID { get; set; }

        [StringLength(150)]
        public string OrgName { get; set; }
    }
}
