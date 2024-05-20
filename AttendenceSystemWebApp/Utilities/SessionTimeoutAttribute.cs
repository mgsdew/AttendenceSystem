using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace AttendenceSystemWebApp.Utilities
{

    public class SessionTimeoutAttribute : ActionFilterAttribute
    {

       
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext _HttpContext = HttpContext.Current;
            if (HttpContext.Current.Session["User"] != null)
            {

                filterContext.Result = new RedirectResult("~/Attendance/SignOut");
                return;

            }
            ////

            base.OnActionExecuting(filterContext);
        }
    }

}