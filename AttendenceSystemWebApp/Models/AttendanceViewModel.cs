using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AttendenceSystemWebApp.Models
{
    public class AttendanceViewModel
    {
        [Display(Name = "Employee Code")]
        public string EmpCod { get; set; }      
        public string FirstName { get; set; }      
        public string LastName { get; set; }      
        
        public int Id { get; set; }

        public int? EId { get; set; }
        public string DateTime { get; set; }

        [Display(Name = "Late Time")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Duration)]
        public TimeSpan LateTime { get; set; }
        [Display(Name = "Check Status")]
        [Required]
        public string CheckStatus { get; set; }
        [Display(Name = "Late Start")]
        public bool LateStart { get; set; }
        [Display(Name = "Early Out")]
        public bool EarlyOut { get; set; }
        public string Remarks { get; set; }

        [Display(Name = "Image")]
        public string CheckInImageUrl { get; set; }
        public string CheckOutImageUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
        public string Showroom { get; set; }

        [Display(Name = "Updated Date")]
        public DateTimeOffset? UpdatedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
       
        public DateTimeOffset EntryTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTimeOffset? EndTime { get; set; }
    }
}