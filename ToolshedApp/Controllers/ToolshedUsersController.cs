using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToolshedApp.Models;
using Microsoft.AspNet.Identity;


namespace ToolshedApp.Controllers
{
    public class ToolshedUsersController : Controller
    {
        private ToolshedContext db = new ToolshedContext();

        public ToolshedRepository Repo { get; set; }

        public ToolshedUsersController() : base()
        {
            Repo = new ToolshedRepository();
        }

        // GET: ToolshedUsers
        public ActionResult Index()
        {
            return View(db.ToolshedUsers.ToList());
        }

        // GET: ToolshedUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToolshedUser toolshedUser = db.ToolshedUsers.Find(id);
            if (toolshedUser == null)
            {
                return HttpNotFound();
            }
            return View(toolshedUser);
        }

        // GET: ToolshedUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToolshedUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,UserName,Phone,Street")] ToolshedUser toolshedUser)
        {
            string user_id = User.Identity.GetUserId();
            ApplicationUser real_user = Repo.Context.Users.FirstOrDefault(u => u.Id == user_id);


            if (ModelState.IsValid)
            {
                Repo.CreateToolshedUser(real_user, toolshedUser.FirstName, toolshedUser.LastName, toolshedUser.UserName, toolshedUser.Phone, toolshedUser.Street);
                return RedirectToAction("_MyTools", "Tools");
            }

            return RedirectToAction("Create", "ToolshedUsers" );
        }

        // GET: ToolshedUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToolshedUser toolshedUser = db.ToolshedUsers.Find(id);
            if (toolshedUser == null)
            {
                return HttpNotFound();
            }
            return View(toolshedUser);
        }

        // POST: ToolshedUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,UserName,Phone,Street")] ToolshedUser toolshedUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toolshedUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toolshedUser);
        }

        // GET: ToolshedUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToolshedUser toolshedUser = db.ToolshedUsers.Find(id);
            if (toolshedUser == null)
            {
                return HttpNotFound();
            }
            return View(toolshedUser);
        }

        // POST: ToolshedUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ToolshedUser toolshedUser = db.ToolshedUsers.Find(id);
            db.ToolshedUsers.Remove(toolshedUser);
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
