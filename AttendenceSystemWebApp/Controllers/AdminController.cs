using AttendenceSystemWebApp.DataAccessLayer.Gateway;
using AttendenceSystemWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AttendenceSystemWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public SqlRepository<AttndRegDet> _context;
        public SqlRepository<Designation> _desgContext;
        ApplicationDbContext db = new ApplicationDbContext();


        public AdminController(SqlRepository<AttndRegDet> attendanceContext, SqlRepository<Designation> desgContext)
        {
            this._context = attendanceContext;
            this._desgContext = desgContext;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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
        
        [HttpGet]
        public ActionResult Create()
        {
            DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();

            AttndRegDet attendance = new AttndRegDet();
            attendance.EntryTime = DateTimeOffset.UtcNow.ToLocalTime();
            //attendance.EndTime = DateTimeOffset.UtcNow.ToLocalTime() ;
            ViewBag.Employee = dbset.Select(x => new { x.UserName }).ToList();
            attendance.CheckStatus = "Check In";
            AttndRegDet lastEmployeeCheckedInbyId = _context.Collection()
                                    .Where(m => (m.EmpCod == attendance.EmpCod) & (m.CheckStatus == "Check In"))
                                    .OrderByDescending(m => m.EntryTime)
                                    .FirstOrDefault();
            if (lastEmployeeCheckedInbyId != null && lastEmployeeCheckedInbyId.EntryTime.Date == DateTimeOffset.Now.Date)
            {
                attendance.CheckStatus = "Check Out";
            }
            return View(attendance);

        }
        [HttpPost]
        public ActionResult Create(AttndRegDet attendance)
        {
            //DateTime createdDate = attendance.CreatedDate;
            DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();
            ViewBag.Employee = dbset.Select(x => new { x.UserName }).ToList();
            var currentTime = DateTime.Now.TimeOfDay;
            var start = new TimeSpan(9, 15, 0);
            var End = new TimeSpan(17, 0, 0);

            if (attendance.CheckStatus == "Check In")
            {
                if (attendance.EntryTime.TimeOfDay > start)
                {
                    attendance.LateTime = (attendance.EntryTime.TimeOfDay - start);
                    attendance.LateStart = true;

                }
                else
                {

                    attendance.LateStart = false;
                }

            }

            if (attendance.CheckStatus == "Check Out")
            {
                
                if (attendance.EndTime == null)
                {
                    ViewBag.Message = "You are checking out..So,End Time Required";
                    return View();
                }
                string strDate = Convert.ToString(attendance.EndTime);
                DateTimeOffset endDate = Convert.ToDateTime(strDate);
                if (End > endDate.TimeOfDay)
                {
                    
                    //attendance.LateTime = (attendance.EndTime - attendance.EntryTime.TimeOfDay);
                    attendance.EarlyOut = true;
                    //attendance.LateTime = "00:00:00";

                }
                else
                {
                    attendance.EarlyOut = false;

                }
            }
            AttndRegDet lastEmployeeCheckedInbyId = new AttndRegDet();
            lastEmployeeCheckedInbyId = _context.Collection()
                        .Where(m => (m.EmpCod == attendance.EmpCod) & (m.CheckStatus == "Check In"))
                        .OrderByDescending(m => m.EntryTime)
                        .FirstOrDefault();
            AttndRegDet lastEmployeeCheckedOutbyId = new AttndRegDet();
            lastEmployeeCheckedOutbyId = _context.Collection()
                        .Where(m => (m.EmpCod == attendance.EmpCod) & (m.CheckStatus == "Check Out"))
                        .OrderByDescending(m => m.EntryTime)
                        .FirstOrDefault();

            if (attendance.CheckStatus == "Check Out")
            {
                if (lastEmployeeCheckedOutbyId != null)
                {
                    if (lastEmployeeCheckedOutbyId.EntryTime.Day == attendance.EntryTime.Day)
                    {
                        ViewBag.Message = "Sorry! This Employee Has already Checked In and Checked Out,please try from next working day";
                        return View();
                    }
                }

            }
            else if (attendance.CheckStatus == "Check In")
            {
                if (lastEmployeeCheckedInbyId != null)
                {
                    if ((lastEmployeeCheckedInbyId.EntryTime.Day == attendance.EntryTime.Day) && lastEmployeeCheckedInbyId.CheckStatus == "Check In")
                    {
                        ViewBag.Message = "Sorry! This Employee has already Checked In,try from another day";
                        return View();
                    }
                }
                else if (lastEmployeeCheckedOutbyId != null)
                {
                    if ((lastEmployeeCheckedOutbyId.EntryTime.Day == attendance.EntryTime.Day))
                    {
                        ViewBag.Message = "Sorry! This Employee already Checked In and out,try from another day";
                        return View();
                    }
                }
               

            }
            string adminId = User.Identity.GetUserId();
            var adminInfo = dbset.Where(x => x.Id == adminId).Select(x => new { x.FirstName, x.LastName }).Single();
            attendance.CreatedBy = adminInfo.FirstName + " " + adminInfo.LastName;

            if (attendance.CheckStatus == "Check Out")
            {
                if(lastEmployeeCheckedInbyId == null){
                    ViewBag.Message = "Sorry! This Employee didn't checked in yet,please check in first";
                    return View();
                }
                else if (lastEmployeeCheckedInbyId.EntryTime.Day == attendance.EntryTime.Day && lastEmployeeCheckedInbyId != null)
                {
                    var editToAttendance = _context.Collection().Where(x => (x.EmpCod == attendance.EmpCod) && (x.EntryTime.Day == attendance.EntryTime.Day)).Single();
                    if (editToAttendance != null)
                    {
                        editToAttendance.CheckStatus = "Check Out";
                        editToAttendance.EarlyOut = attendance.EarlyOut;
                        editToAttendance.CreatedBy = attendance.CreatedBy;
                        editToAttendance.EndTime = attendance.EndTime;
                        editToAttendance.UpdatedBy = attendance.CreatedBy;

                        //_context.Update(editToAttendance);

                        _context.Commit();
                        ViewBag.Message = "Attendance has been successfully recorded.";

                        return View();
                    }

                }
            }
            _context.Insert(attendance);
            _context.Commit();


            ViewBag.Message = "Attendance has been successfully recorded.";
            return View();

        }

        public JsonResult GetEmployeeByCode(string id)
        {
            ApplicationUser user = new ApplicationUser();
            if (id != "")
            {
                DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();

                user = dbset.FirstOrDefault(x => x.UserName == id);

                AttndRegDet lastEmployeeCheckedInbyId = _context.Collection()
                                        .Where(m => (m.EmpCod == id) & (m.CheckStatus == "Check In"))
                                        .OrderByDescending(m => m.EntryTime)
                                        .FirstOrDefault();
                Designation designation = new Designation();


                designation.Desg = "Not Found";
                if (user.DesignationId != null)
                {
                    designation = _desgContext.Collection().FirstOrDefault(x => x.DesgId == user.DesignationId);

                }

                string checkSt;
                checkSt = "Check In";
                if (lastEmployeeCheckedInbyId != null)
                {
                    checkSt = "Check Out";
                }
                var emp = new
                {
                    EmpName = user.FirstName + " " + user.LastName,
                    Mail = user.Email,
                    CheckStatus = checkSt,
                    Designation = designation.Desg
                };

                return Json(emp);
            }
            else
            {
                var emp = new
                {
                    EmpCod = "",
                    EmpName = "",
                    Mail = "",
                    CheckStatus = "",
                    Designation = ""
                };

                return Json(emp);
            }
        }

    }
}