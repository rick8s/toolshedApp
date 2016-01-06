using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToolshedApp.Models;

namespace ToolshedApp.Content
{
    public class ToolsController : Controller
    {
        private ToolshedContext db = new ToolshedContext();

        // GET: Tools
        public ActionResult Index()
        {
            return View(db.Tools.ToList());
        }

        // GET: Tools/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tool tool = db.Tools.Find(id);
            if (tool == null)
            {
                return HttpNotFound();
            }
            return View(tool);
        }

        // GET: Tools/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ToolId,Name,Category,Description,Image,Available")] Tool tool)
        {
            if (ModelState.IsValid)
            {
                db.Tools.Add(tool);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tool);
        }

        // GET: Tools/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tool tool = db.Tools.Find(id);
            if (tool == null)
            {
                return HttpNotFound();
            }
            return View(tool);
        }

        // POST: Tools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ToolId,Name,Category,Description,Image,Available")] Tool tool)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tool).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tool);
        }

        // GET: Tools/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tool tool = db.Tools.Find(id);
            if (tool == null)
            {
                return HttpNotFound();
            }
            return View(tool);
        }

        // POST: Tools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tool tool = db.Tools.Find(id);
            db.Tools.Remove(tool);
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
