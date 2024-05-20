using System;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendenceSystemWebApp.Models;
using AttendenceSystemWebApp.Interfaces;
using AttendenceSystemWebApp.DataAccessLayer.Gateway;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AttendenceSystemWebApp.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {



        public SqlRepository<Dept> departmentContext;
        public SqlRepository<Designation> designationContext;
        public SqlRepository<Entity> _entityContext;
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public EmployeeController(SqlRepository<Dept> deptContext,
            SqlRepository<Designation> desigContext,
            SqlRepository<Entity> entityContext)

        {

            departmentContext = deptContext;
            designationContext = desigContext;
            _entityContext = entityContext;

        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetEmployee(string Department, int? EId)
        {
            DbSet<Entity> _entityContext = db.Set<Entity>();
            ViewBag.Entity = _entityContext.ToList();
            DbSet<Dept> _deptContext = db.Set<Dept>();
            ViewBag.Dept = _deptContext.ToList();
            if (EId > 0)
            {
                var employees = db.Users.Where(x => x.EntityId == EId).Include(x => x.Dept).Include(x => x.Designation).Include(x => x.Entity).ToList();
                return View(employees);
            }
            else if (!string.IsNullOrEmpty(Department))
            {
                var employees = db.Users.Where(x => x.DeptId == Department).Include(x => x.Dept).Include(x => x.Designation).Include(x => x.Entity).ToList();
                return View(employees);
            }
            else if (EId > 0 && !string.IsNullOrEmpty(Department))
            {
                var employees = db.Users.Where(x => (x.EntityId == EId) && (x.DeptId == Department)).Include(x => x.Dept).Include(x => x.Designation).Include(x => x.Entity).ToList();
                return View(employees);
            }
            else
            {
                var employees = db.Users.Include(x => x.Dept).Include(x => x.Designation).Include(x => x.Entity).ToList();
                return View(employees);
            }



        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditEmployee(string id)
        {
            

            ViewBag.Designations = designationContext.Collection().ToList();
            ViewBag.Departments = departmentContext.Collection().ToList();
            ViewBag.Entities = _entityContext.Collection().ToList();
            var user = db.Users.FirstOrDefault(x => x.Id == id);


            user.PasswordHash = null;
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditEmployee(ApplicationUser user, string id)
        {
            ViewBag.Designations = designationContext.Collection().ToList();
            ViewBag.Departments = departmentContext.Collection().ToList();


            if (user.UserName != null)
            {
                DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();
                ApplicationUser getEmployeeByEmpCode = db.Users.FirstOrDefault(x => x.UserName == user.UserName);
                if (getEmployeeByEmpCode != null && getEmployeeByEmpCode.Email != user.Email && getEmployeeByEmpCode.Id != user.Id)
                {
                    ViewBag.Message = "The Employee Code Already Used";
                    return View(user);
                }
                var userToEdit = db.Users.FirstOrDefault(x => x.Id == user.Id);

                if (!ModelState.IsValid)
                {
                    return View(user);
                }
                else
                {
                    //var manager = new ApplicationUserManager();
                    if (user.PasswordHash != null)
                    {
                        var token = UserManager.GeneratePasswordResetToken(user.Id);
                        UserManager.ResetPassword(user.Id, token, user.PasswordHash);
                    }


                    userToEdit.UserName = user.UserName;
                    userToEdit.Email = user.Email;
                    userToEdit.PhoneNumber = user.PhoneNumber;
                    userToEdit.EmgContactNo = user.EmgContactNo;
                    userToEdit.EmgContactPerson = user.EmgContactPerson;
                    userToEdit.Father = user.Father;
                    userToEdit.Mother = user.Mother;
                    userToEdit.Sex = user.Sex;
                    userToEdit.SuprvisorId = user.SuprvisorId;
                    userToEdit.DeptId = user.DeptId;
                    userToEdit.EntityId = user.EntityId;
                    userToEdit.DesignationId = user.DesignationId;
                    userToEdit.DOB = user.DOB;
                    userToEdit.FirstName = user.FirstName;
                    userToEdit.LastName = user.LastName;
                    userToEdit.ParmanentAdd = user.ParmanentAdd;
                    userToEdit.PresentAdd = user.PresentAdd;
                    userToEdit.NID = user.NID;
                    userToEdit.BloodGr = user.BloodGr;
                    userToEdit.PCName = user.PCName;
                    userToEdit.Longitude = user.Longitude;
                    userToEdit.Latitude = user.Latitude;
                    //dbset.Add(user);

                    db.SaveChanges();

                    return RedirectToAction("GetEmployee");
                }
            }
            else
            {
                ViewBag.Message = "Employee Code Should not Be Empty";
                return View(user);
            }

        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string Id)
        {
            DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();

            ApplicationUser employee = dbset.Find(Id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(employee);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult ConfirmDelete(string Id)
        {
            DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();

            ApplicationUser employeeToDelete = dbset.Find(Id);

            if (employeeToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (db.Entry(employeeToDelete).State == EntityState.Detached)
                {
                    dbset.Attach(employeeToDelete);
                }

                dbset.Remove(employeeToDelete);
                db.SaveChanges();
                return RedirectToAction("GetEmployee");
            }
        }

        [HttpGet]
        public ActionResult Details(string Id)
        {
            DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();

            ApplicationUser employee = dbset.Find(Id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(employee);
            }
        }
    }
}