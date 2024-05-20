using AttendenceSystemWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendenceSystemWebApp.DataAccessLayer.Gateway;
using System.Device.Location;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Drawing;
using System.Web.Hosting;
using System.Net.NetworkInformation;
using System.Net;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AttendenceSystemWebApp.Utilities;
using PagedList;


namespace AttendenceSystemWebApp.Controllers
{
    [Authorize]
    [SessionTimeout]
    public class AttendanceController : Controller
    {

        public SqlRepository<AttndRegDet> _context;

        public SqlRepository<Designation> _desgContext;
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AttendanceController(SqlRepository<AttndRegDet> attendanceContext, SqlRepository<Designation> desgContext)
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
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        [HttpGet]
        public ActionResult SignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session["User"] = "";
            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "Employee")]
        public ActionResult AttendanceListByEmployee()
        {
            DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();
            string UserId = User.Identity.GetUserId();

            var attendances = from a in db.AttndRegDet.ToList()
                              join e in dbset.ToList()
                              on a.EmpCod equals e.UserName
                              where e.Id == UserId
                              orderby a.EntryTime descending
                              select new AttendanceViewModel
                              {
                                  Id = a.Id,
                                  CheckInImageUrl = a.CheckInImageUrl,
                                  CheckOutImageUrl = a.CheckOutImageUrl,
                                  FirstName = e.FirstName,
                                  LastName = e.LastName,
                                  EmpCod = a.EmpCod,
                                  CheckStatus = a.CheckStatus,
                                  LateTime = a.LateTime,
                                  LateStart = a.LateStart,
                                  EndTime = a.EndTime,
                                  EarlyOut = a.EarlyOut,
                                  EntryTime = a.EntryTime,
                                  CreatedBy = a.CreatedBy,
                                  UpdatedBy = a.UpdatedBy,
                                  Remarks = a.Remarks
                              };
            return View(attendances.ToList());
        }

        //[Authorize(Roles = "Employee")]
        public ActionResult AddAttendance()
        {
            DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();

            AttndRegDet attendance = new AttndRegDet();
            //ViewBag.Employee = dbset.Select(x => new { x.EmpCod }).ToList();
            var user = dbset.Find(User.Identity.GetUserId());
            attendance.EmpCod = user.UserName;
            //attendance.EmpName = user.FirstName +" "+ user.LastName;
            attendance.CheckStatus = "Check In";
            AttndRegDet lastEmployeeCheckedInbyId = _context.Collection()
                                    .Where(m => (m.EmpCod == attendance.EmpCod) & (m.CheckStatus == "Check In"))
                                    .OrderByDescending(m => m.EntryTime)
                                    .FirstOrDefault();
            if (lastEmployeeCheckedInbyId != null && lastEmployeeCheckedInbyId.EntryTime.Date == DateTimeOffset.Now.Date)
            {
                attendance.CheckStatus = "Check Out";
            }
            var u = dbset.Where(x=>x.UserName == attendance.EmpCod).Select(x=> new{ x.Email , x.DesignationId, x.FirstName,x.LastName}).Single();
            ViewBag.Name = u.FirstName + " " + u.LastName;
            ViewBag.Email = u.Email;
            ViewBag.Desg = "Not Found";
            //ViewBag.Mac = u.MacAddress;

            if (u.DesignationId != null)
            {
                var desg = _desgContext.Collection().Where(x => x.DesgId == u.DesignationId).Single();
                ViewBag.Desg = desg.Desg;

            }
            return View(attendance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttendance(AttndRegDet attendance, string ImageUrl)
        {

            //attendance.EntryTime = DateTimeOffset.Now;
            if (ImageUrl == null)
            {
                ViewBag.Message = "Wait for a second, Live camera is opening";
                return View();
            }
            else
            {
                DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();
                attendance.CreatedBy = User.Identity.GetUserName();
                //DateTime createdDate = Convert.ToDateTime(attendance.CreatedDate);
                var u = dbset.Where(x => x.UserName == attendance.EmpCod).Select(x => new { x.Email, x.DesignationId, x.FirstName, x.LastName, x.EntityId }).Single();
                ViewBag.Name = u.FirstName + " " + u.LastName;
                ViewBag.Email = u.Email;
                ViewBag.Desg = "Not Found";

                if (u.DesignationId != null)
                {
                    var desg = _desgContext.Collection().Where(x => x.DesgId == u.DesignationId).Single();
                    ViewBag.Desg = desg.Desg;

                }
                var currentTime = DateTime.Now.TimeOfDay;
                var start = new TimeSpan(9, 30, 0);
                var End = new TimeSpan(21, 0, 0);
                if (u.EntityId == 139 || u.EntityId == 143 || u.EntityId == 33 || u.EntityId == 137)
                {
                    //bc,ss,new market ctg,jfp
                    //ss tuesday 11.00 start
                    start = new TimeSpan(10, 30, 0);
                    if (u.EntityId == 139 && attendance.EntryTime.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        start = new TimeSpan(11, 30, 0);
                    }
                }


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
                    if (End > attendance.EntryTime.TimeOfDay)
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
                            ViewBag.Message = "Sorry! You Have already Checked In and Checked Out,please try from next working day";
                            return View();
                        }
                    }
                    
                }                
                else if(attendance.CheckStatus == "Check In")
                {
                    
                    if ((lastEmployeeCheckedOutbyId != null) && (lastEmployeeCheckedOutbyId.EntryTime.Day == attendance.EntryTime.Day))
                    {
                        
                            ViewBag.Message = "Sorry! You Have already Checked In and out,try from another day";
                            return View();
                        
                    }else if (lastEmployeeCheckedInbyId != null)
                    {
                        if ((lastEmployeeCheckedInbyId.EntryTime.Day == attendance.EntryTime.Day) && lastEmployeeCheckedInbyId.CheckStatus == "Check In")
                        {
                            ViewBag.Message = "Sorry! You Have already Checked In,try from another day";
                            return View();
                        }
                    }
                    //}else if (lastEmployeeCheckedOutbyId != null)
                    //{
                    //    ViewBag.Message = "Sorry! You didn't checked in yet,please check in first";
                    //    return View();
                    //}
                    string fileName = attendance.EmpCod + DateTime.Now.ToString("_dd-MM-yy_hh-mm-ss") + ".jpeg";
                    SaveDataUrlToFile(ImageUrl, fileName);
                    attendance.CheckInImageUrl = fileName;

                }
                 

                if (attendance.CheckStatus == "Check Out")
                {
                    if (lastEmployeeCheckedInbyId == null)
                    {
                        ViewBag.Message = "Sorry! You didn't checked in yet,please check in first";
                        return View();
                    }
                    else if ((lastEmployeeCheckedOutbyId != null) && (lastEmployeeCheckedOutbyId.EntryTime.Day == attendance.EntryTime.Day))
                    {
                        
                            ViewBag.Message = "Sorry! You Have already Checked In and out,try from another day";
                            return View();
                
                    }
                    else if (lastEmployeeCheckedInbyId.EntryTime.Day == attendance.EntryTime.Day && lastEmployeeCheckedInbyId != null)
                    {
                        
                        var editToAttendance = _context.Collection().Where(x => (x.EmpCod == attendance.EmpCod) && ( x.EntryTime.Day == attendance.EntryTime.Day)&&(x.EntryTime.Month==attendance.EntryTime.Month)).FirstOrDefault();
                        if (editToAttendance != null)
                        {
                            editToAttendance.CheckStatus = "Check Out";
                            editToAttendance.EarlyOut = attendance.EarlyOut;
                            editToAttendance.CreatedBy = attendance.CreatedBy;
                            editToAttendance.EndTime = DateTimeOffset.Now;

                            string fileName = attendance.EmpCod + DateTime.Now.ToString("_dd-MM-yy_hh-mm-ss") + ".jpeg";
                            SaveDataUrlToFile(ImageUrl, fileName);
                            editToAttendance.CheckOutImageUrl = fileName;
                            //_context.Update(editToAttendance);

                            _context.Commit();
                            ViewBag.Message = "Your attendance has been successfully recorded.";

                            return View();
                        }

                    }
                }
                _context.Insert(attendance);
                _context.Commit();

                ViewBag.Message = "Your attendance has been successfully recorded.";
                
                return View();
            }
            
        }

        public void SaveDataUrlToFile(string dataUrl, string filename)
        {
            var matchGroups = Regex.Match(dataUrl, @"^data:((?<type>[\w\/]+))?;base64,(?<data>.+)$").Groups;
            var base64Data = matchGroups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);
            System.IO.File.WriteAllBytes(Server.MapPath("~/Content/Images/" + filename), binData);
            //System.IO.File.WriteAllBytes(Server.MapPath("C://home//site//Images//" + filename), binData);
        }
        [HttpGet]
        public ActionResult Details(int Id)
        {
            //DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();

            AttndRegDet attnd = _context.Find(Id);
            if (attnd == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(attnd);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int Id)
        {

            AttndRegDet attndRegDet = _context.Find(Id);
            
            if (attndRegDet == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(attndRegDet);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(AttndRegDet attndRegDet, int Id)
        {
            DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();


            AttndRegDet attndToEdit = _context.Find(Id);

            string adminId = User.Identity.GetUserId();
            var adminInfo = dbset.Where(x => x.Id == adminId).Select(x => new { x.FirstName, x.LastName }).Single();
            attndRegDet.UpdatedBy = adminInfo.FirstName + " " + adminInfo.LastName;
            attndRegDet.UpdatedDate = DateTimeOffset.UtcNow;

            if (attndToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
               
                attndToEdit.EmpCod = attndRegDet.EmpCod;
                attndToEdit.LateStart = attndRegDet.LateStart;
                attndToEdit.LateTime = attndRegDet.LateTime;
                attndToEdit.Longitude = attndRegDet.Longitude;
                attndToEdit.Latitude = attndRegDet.Latitude;
                attndToEdit.EntryTime = attndRegDet.EntryTime;
                attndToEdit.EndTime = attndRegDet.EndTime;
                attndToEdit.EarlyOut = attndRegDet.EarlyOut;
                attndToEdit.UpdatedBy = attndRegDet.UpdatedBy;
                attndToEdit.UpdatedDate = attndRegDet.UpdatedDate;
                attndToEdit.CheckStatus = attndRegDet.CheckStatus;
                attndToEdit.Remarks = attndRegDet.Remarks;
                if (User.Identity.GetUserName()=="Emp001")
                {
                    attndToEdit.CheckInImageUrl = attndRegDet.CheckInImageUrl;
                    attndToEdit.CheckOutImageUrl = attndRegDet.CheckOutImageUrl;
                }

                _context.Commit();

                return RedirectToAction("GetAttendances");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            //DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();

            AttndRegDet attnd = _context.Find(Id);
            if (attnd == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(attnd);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int Id)
        {
            AttndRegDet attndToEdit = _context.Find(Id);

            if (attndToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                _context.Delete(Id);
                _context.Commit();
                
                return RedirectToAction("GetAttendances");
            }
        }
                
        [HttpGet]
        [Authorize(Roles = "Admin, General Admin")]
        public ActionResult GetAttendances(int? EId, string DateTo,string DateFrom, string sortOrder, string currentFilter, string searchString, int? page)
        {
            string query = "EXEC GetAttendances";

            IEnumerable<AttendanceViewModel> attendances = db.Database.SqlQuery<AttendanceViewModel>(query);
            //IEnumerable<AttendanceViewModel> attendances = 
            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            

            ViewBag.CurrentFilter = searchString;


            DbSet<ApplicationUser> dbset = db.Set<ApplicationUser>();
            DbSet<Entity> _entityContext = db.Set<Entity>();
            ViewBag.Entity = _entityContext.ToList();

            if (EId > 0 && !String.IsNullOrEmpty(DateTo) && !String.IsNullOrEmpty(DateFrom))
            {
                var sh = _entityContext.Where(x => x.EID == EId).Select(x => x.eName).Single().ToString();
                attendances = attendances.Where(x => (x.Showroom == sh)&&(x.EntryTime.Date >= Convert.ToDateTime(DateFrom).Date)&&(x.EntryTime.Date <= Convert.ToDateTime(DateTo).Date));
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                //ViewBag.Attendances = attendances.ToPagedList(pageNumber, pageSize);

                return View(attendances.ToPagedList(pageNumber, pageSize));
            }else if (EId == 0 && !String.IsNullOrEmpty(DateTo) && !String.IsNullOrEmpty(DateFrom))
            {
                //var sh = _entityContext.Where(x => x.EID == EId).Select(x => x.eName).Single().ToString();
                attendances = attendances.Where(x => (x.EntryTime.Date >= Convert.ToDateTime(DateFrom).Date) && (x.EntryTime.Date <= Convert.ToDateTime(DateTo).Date));
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                //ViewBag.Attendances = attendances.ToPagedList(pageNumber, pageSize);

                return View(attendances.ToPagedList(pageNumber, pageSize));
            }
            else if (EId > 0 && String.IsNullOrEmpty(DateTo) && String.IsNullOrEmpty(DateFrom))
            {
                var sh = _entityContext.Where(x => x.EID == EId).Select(x => x.eName).Single().ToString();
                attendances = attendances.Where(x => x.Showroom == sh);
                
                int pageSize = 25;
                int pageNumber = (page ?? 1);
                //ViewBag.Attendances = attendances.ToPagedList(pageNumber, pageSize);

                return View(attendances.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                
                int pageSize = 10;
                int pageNumber = (page ?? 1);             

                
                return View(attendances.ToPagedList(pageNumber, pageSize));
            }


        }        

    }
}