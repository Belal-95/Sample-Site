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
    public class tblCommentsController : Controller
    {
        private MyBlogEntities1 db = new MyBlogEntities1();

        // GET: tblComments
        public ActionResult Index()
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Login");
            }

            var tblComments = db.tblComments.Include(t => t.tblPost);
            return View(tblComments.ToList());
        }

        // GET: tblComments/Details/5
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
            tblComment tblComment = db.tblComments.Find(id);
            if (tblComment == null)
            {
                return HttpNotFound();
            }
            return View(tblComment);
        }

        // GET: tblComments/Create
        public ActionResult Create()
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.PostId = new SelectList(db.tblPosts, "PostId", "PostTitle");
            return View();
        }

        // POST: tblComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentId,PostId,CommentBody,NameOfUser")] tblComment tblComment)
        {
            if (ModelState.IsValid)
            {
                db.tblComments.Add(tblComment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.tblPosts, "PostId", "PostTitle", tblComment.PostId);
            return View(tblComment);
        }

        // GET: tblComments/Edit/5
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
            tblComment tblComment = db.tblComments.Find(id);
            if (tblComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.tblPosts, "PostId", "PostTitle", tblComment.PostId);
            return View(tblComment);
        }

        // POST: tblComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentId,PostId,CommentBody,NameOfUser")] tblComment tblComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.tblPosts, "PostId", "PostTitle", tblComment.PostId);
            return View(tblComment);
        }

        // GET: tblComments/Delete/5
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
            tblComment tblComment = db.tblComments.Find(id);
            if (tblComment == null)
            {
                return HttpNotFound();
            }
            return View(tblComment);
        }

        // POST: tblComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblComment tblComment = db.tblComments.Find(id);
            db.tblComments.Remove(tblComment);
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
