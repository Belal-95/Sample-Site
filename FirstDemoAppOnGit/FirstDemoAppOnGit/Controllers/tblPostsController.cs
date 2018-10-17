using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirstDemoAppOnGit.Models;

namespace FirstDemoAppOnGit.Controllers
{
    public class tblPostsController : Controller
    {
        private MyBlogEntities1 db = new MyBlogEntities1();

        // GET: tblPosts
        public ActionResult Index()
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Login");
            }
            return View(db.tblPosts.ToList());
        }

        // GET: tblPosts/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPost tblPost = db.tblPosts.Find(id);
            if (tblPost == null)
            {
                return HttpNotFound();
            }
            return View(tblPost);
        }

        // GET: tblPosts/Create
        public ActionResult Create()
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        // POST: tblPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,PostTitle,PostBody,PostDate,IsVisible")] tblPost tblPost)
        {
            if (ModelState.IsValid)
            {
                db.tblPosts.Add(tblPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblPost);
        }

        // GET: tblPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPost tblPost = db.tblPosts.Find(id);
            if (tblPost == null)
            {
                return HttpNotFound();
            }
            return View(tblPost);
        }

        // POST: tblPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,PostTitle,PostBody,PostDate,IsVisible")] tblPost tblPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblPost);
        }

        // GET: tblPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPost tblPost = db.tblPosts.Find(id);
            if (tblPost == null)
            {
                return HttpNotFound();
            }
            return View(tblPost);
        }

        // POST: tblPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblPost tblPost = db.tblPosts.Find(id);
            db.tblPosts.Remove(tblPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
