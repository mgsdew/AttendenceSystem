using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Device.Location;
namespace AttendenceSystemWebApp.Models
{
    [Table("AttndRegDet")]
    public class AttndRegDet : BaseEntity
    {

              
        //public string Employee_Id { get; set; }
        [StringLength(12)]
        [Required]
        [Display(Name = "Employee Code")]
        public string EmpCod { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       

        //[Display(Name = "Start Time")]
        //public TimeSpan StartTime { get; set; }
        [Display(Name = "End Time")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTimeOffset? EndTime { get; set; }

        //[StringLength(50)]
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

        [Display(Name = "Updated Date")]
        public DateTimeOffset? UpdatedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Created Date")]
        public DateTimeOffset EntryTime { get; set; }
        public AttndRegDet()
        {
            this.EntryTime = DateTimeOffset.Now;
            //ToString("MM / dd / yyyy hh: mm tt")

            //var st = "09:00 AM";
            //var st2 = "05:00 PM";
            //this.StartTime = DateTimeOffset.Parse(st).ToLocalTime().TimeOfDay;
            //this.EndTime = DateTimeOffset.Parse(st2).ToLocalTime().TimeOfDay;
            //DateTimeOffset st1 = DateTimeOffset.UtcNow.ToLocalTime();



    }
    }




    
}