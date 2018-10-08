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
        MyBlogEntities dbEntities = new MyBlogEntities();

        public ActionResult Index()
        {
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
    }
}