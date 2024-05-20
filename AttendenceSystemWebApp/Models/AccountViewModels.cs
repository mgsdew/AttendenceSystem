using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AttendenceSystemWebApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Employee Code")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        [Display(Name = "Mobile Number")]
        public string PhoneNumber { get; set; }

    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }

       
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Employee Code")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Employee Code")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        [Display(Name = "Mobile Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

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

        [StringLength(3)]
        [Display(Name = "Department")]
        public string DeptId { get; set; }

        [StringLength(3)]
        [Display(Name = "Designation Name")]
        public string DesignationId { get; set; }

        [Display(Name = "Entity")]
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

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
