//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Web;

//namespace AttendenceSystemWebApp.Models
//{
//    [Table("AspNetUsers")]
//    public class AspNetUser:BaseEntity
//    {
//        [Key]
//        [Column(Order = 0)]
//        public string Id { get; set; }

//        [StringLength(256)]
//        public string Email { get; set; }

//        [Key]
//        [Column(Order = 1)]
//        public bool EmailConfirmed { get; set; }

//        public string PasswordHash { get; set; }

//        public string SecurityStamp { get; set; }

//        public string PhoneNumber { get; set; }

//        [Key]
//        [Column(Order = 2)]
//        public bool PhoneNumberConfirmed { get; set; }

//        [Key]
//        [Column(Order = 3)]
//        public bool TwoFactorEnabled { get; set; }

//        public DateTime? LockoutEndDateUtc { get; set; }

//        [Key]
//        [Column(Order = 4)]
//        public bool LockoutEnabled { get; set; }

//        [Key]
//        [Column(Order = 5)]
//        [DatabaseGenerated(DatabaseGeneratedOption.None)]
//        public int AccessFailedCount { get; set; }

//        [Key]
//        [Column(Order = 6)]
//        [StringLength(256)]
//        public string UserName { get; set; }

//        [StringLength(128)]
//        public string FirstName { get; set; }

//        [StringLength(128)]
//        public string LastName { get; set; }

//        [StringLength(8)]
//        public string EmpCod { get; set; }

//        [StringLength(50)]
//        public string Father { get; set; }

//        [StringLength(50)]
//        public string Mother { get; set; }

//        public DateTime? DOB { get; set; }

//        [StringLength(25)]
//        public string Sex { get; set; }

//        [StringLength(3)]
//        public string DeptId { get; set; }

//        [StringLength(3)]
//        public string DesgId { get; set; }

//        [StringLength(50)]
//        public string Phone { get; set; }

//        [StringLength(250)]
//        public string PresentAdd { get; set; }

//        [StringLength(250)]
//        public string ParmanentAdd { get; set; }

//        [StringLength(50)]
//        public string EmgContactPerson { get; set; }

//        [StringLength(50)]
//        public string EmgContactNo { get; set; }

//        [StringLength(10)]
//        public string SuprvisorId { get; set; }

//        [StringLength(15)]
//        public string BloodGr { get; set; }

//        [StringLength(50)]
//        public string NID { get; set; }
//    }
//}