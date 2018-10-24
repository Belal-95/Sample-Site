using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FirstDemoAppOnGit.Common;
using FirstDemoAppOnGit.Models;

namespace FirstDemoAppOnGit.Controllers
{
    public class HomeController : Controller
    {
        MyBlogEntities1 dbEntities = new MyBlogEntities1();

        public ActionResult Index()
        {
            if(Session["EmailId"] == null )
            {
               return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LogIn()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            // Here i changed the return type of the SP to make it return a model type ("tblAdminAccount" model type) 
            List<tblAdminAccount> objAdminAccount = dbEntities.SP_CheckAdminLogin(email, Utility.Encrypt(password, "ER")).ToList();

            if (objAdminAccount.Count > 0)
            {
                //if (dbEntities.SP_CheckUserAccountStatus(email, 1).FirstOrDefault().Value > 0)
                //{
                //    ViewBag.Status = "Please check your mail to activate the account!!!";
                //    ViewBag.Activation = 1;
                //    TempData["Email"] = email;
                //    return View();
                //}
                //if (dbEntities.SP_CheckUserAccountStatus(email, 2).FirstOrDefault().Value > 0)
                //{
                //    ViewBag.Status = "Your Account has been cancelled. Please contact to Administration!!!";
                //    return View();
                //}

                //Session["UserId"] = objAdminAccount[0].UserId;

                //Session["FirstName"] = objAdminAccount[0].tblUserPersonalDetails.FirstOrDefault().FirstName;

                Session["EmailId"] = objAdminAccount[0].Emailid;

                string lastAccessedDateTime = objAdminAccount[0].LastAccessedDateTime.ToString();

                DateTime currentAccessedDateTime = DateTime.Now;

                if (string.IsNullOrEmpty(lastAccessedDateTime))
                {
                    Session["LastAccessedDateTime"] = currentAccessedDateTime.ToString();
                }
                else
                {
                    Session["LastAccessedDateTime"] = lastAccessedDateTime;
                }

                //tblUserAccountDetail objUserAccountDetail = dbEntities.tblUserAccountDetails.Find(objAdminAccount[0].UserId);
                tblAdminAccount tblObjAdminAccount = dbEntities.tblAdminAccounts.Find(objAdminAccount[0].Emailid);
                tblObjAdminAccount.LastAccessedDateTime = currentAccessedDateTime;

                dbEntities.SaveChanges();
                ////////////
                //string s = Session["ToPage"].ToString();
                //if (Session["ToPage"] == "MyReminders")

                //    return RedirectToAction(s, "Home", new { area = "User" });
                ///////////////

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Status = "Invalid Email Address or Password";
            }
            return View();
        }

        public JsonResult CheckEmailAvailability(string email)
        {
            return Json(dbEntities.SP_CheckEmailAvailability(email),JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendForgetPassword(string email)
        {

            int status = 1;
            try
            {
                Random objRandom = new Random();
                int num = objRandom.Next(999999999);
                DateTime dt = DateTime.Now;
                string password = num.ToString() + dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString();
                dbEntities.SP_UpdatPasswordByEmail(email,Utility.Encrypt(password,"ER"));

                string message = string.Empty;

                message += "<table style='width:700px;border:10px outset blue;border-redius:30px'>";

                message += "<tr><td><img src='https://i0.wp.com/www.anphira.com/wp-content/uploads/2013/01/header.jpg' width=700px alt='Reset your Password' /></td></tr>";

                message += "<tr style='background-color:yellow color:green'><td><b>Dear Admin, </b><br /><br />";

                message += " Your new Password is: " + password + "<br /><br />";

                message += "<b>Thanks & Best Regards,</b><br />Belal - Blog<br />Managment Team<br />Damascus Syria</td></tr></table>";

                Utility.SendEmail("Belal - Blog  Reset Password", email, message, true);
            }
            catch
            {
                status = 0;
            }

            return Json(status,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Remove("c");
            Session.Remove("LastAccessDateTime");

            return RedirectToAction("Login");
        }
        public ActionResult ChangePassword()
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public JsonResult CheckOldPassword(string oldPwd, string emailId)
        {
            return Json(dbEntities.SP_CheckAdminOldPassword(emailId, Utility.Encrypt(oldPwd, "ER")), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPassword,string newPassword, string newPasswordAgain)
        {
            if (Session["EmailId"] == null)
                return RedirectToAction("Login");

            tblAdminAccount objAdminAccount = dbEntities.tblAdminAccounts.Find(Session["EmailId"].ToString());

            objAdminAccount.password = Utility.Encrypt(newPassword, "ER");

            if (dbEntities.SaveChanges() > 0)
                ViewBag.Status = 1;
            else
                ViewBag.Status = 0;

            return View();
        }

        public ActionResult PostsPage()
        {

            return View(dbEntities.tblPosts.ToList());
        }

        public ActionResult OnePostPage(int? id)
        {

            tblPost tblPost = dbEntities.tblPosts.Find(id);
            // var model = dbEntities.tblPosts.Include("tblComments");
            return View(tblPost);
        }
    }
}