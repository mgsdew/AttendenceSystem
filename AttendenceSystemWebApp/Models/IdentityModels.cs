using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace AttendenceSystemWebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            //this.EntityId = 0;
            Longitude = 0.0;
            Latitude = 0.0;
        }
        // Add new columns
        //[Required]
        //[Display(Name = "Employee Code")]
        //public string EmpCod { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        

        [StringLength(50)]
        [Display(Name = "Father Name")]
        public string Father { get; set; }

        [StringLength(50)]
        [Display(Name = "Mother Name")]

        public string Mother { get; set; }

        [Display(Name = "Date of Birth")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }

        //[StringLength(25)]
        //public string Religon { get; set; }

        [StringLength(25)]
        public string Sex { get; set; }

        public Dept Dept { get; set; }
        [StringLength(3)]
        [Display(Name = "Department")]
        [ForeignKey("Dept")]
        public string DeptId { get; set; }

        public Designation Designation { get; set; }
        [StringLength(3)]
        [Display(Name = "Designation Name")]
        [ForeignKey("Designation")]
        public string DesignationId { get; set; }

        public Entity Entity { get; set; }
        [Display(Name = "Entity")]
        [ForeignKey("Entity")]
        public int EntityId { get; set; }


        [StringLength(250)]
        [Display(Name = "Present Address")]
        public string PresentAdd { get; set; }

        [StringLength(250)]
        [Display(Name = "Parmanent Address")]
        public string ParmanentAdd { get; set; }


        [StringLength(50)]
        [Display(Name = "Emergency Contact Person")]
        public string EmgContactPerson { get; set; }

        [StringLength(50)]
        [Display(Name = "Emergency Contact No")]
        public string EmgContactNo { get; set; }
        [StringLength(10)]
        [Display(Name = "Supervisor")]
        public string SuprvisorId { get; set; }

        [StringLength(15)]
        [Display(Name = "Blood Group")]
        public string BloodGr { get; set; }

        [StringLength(50)]
        [Display(Name = "National Id")]
        public string NID { get; set; }

        [StringLength(50)]
        [Display(Name = "PC Name")]
        public string PCName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<AttndRegDet> AttndRegDet { get; set; }
        public DbSet<Dept> Dept { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<Entity> Entity { get; set; }
        //public DbSet<AspNetUser> AspNetUsers { get; set; }


        public ApplicationDbContext()
            : base("RelAttendance", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}