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



namespace ToolshedApp.Content
{
    public class ToolsController : Controller
    {
        private ToolshedContext db = new ToolshedContext();
        public ToolshedRepository Repo { get; set; }

        public ToolsController() : base()
        {
            Repo = new ToolshedRepository();
        }

        // GET: Tools
        [Authorize]
        public ActionResult Index()
        {

            string user_id = User.Identity.GetUserId();
            ApplicationUser real_user = Repo.Context.Users.FirstOrDefault(u => u.Id == user_id);
            ToolshedUser me = null;
            try
            {
                me = Repo.GetAllUsers().Where(u => u.RealUser.Id == user_id).Single();

            }
            catch (Exception)
            {
                bool successful = Repo.AddNewUser(real_user);
            }



            List<Tool> my_tools = Repo.GetAvailableTools();
            return View(my_tools);
        }

        // GET: Tools/Details/5
        public ActionResult Borrow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tool tool = Repo.Context.Tools.Find(id);
            if (tool == null)
            {
                return HttpNotFound();
            }
            tool.Available = false;
            Repo.Context.SaveChanges();
            return View("Index");
        }

        // GET: Tools/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Owner,ToolId,Name,Category,Description,Image")] Tool tool)
        
        {
            string user_id = User.Identity.GetUserId();
            ApplicationUser real_user = Repo.Context.Users.FirstOrDefault(u => u.Id == user_id);
            ToolshedUser me = Repo.GetAllUsers().Where(u => u.RealUser.Id == user_id).SingleOrDefault();

            if (ModelState.IsValid)
            {
                Repo.CreateTool(me, tool.Name, tool.Category,tool.Description, tool.Image);                
            }
        
            return RedirectToAction("Index");
        }

        // GET: Tools/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tool tool = Repo.Context.Tools.Find(id);
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
                Repo.Context.Entry(tool).State = EntityState.Modified;
                Repo.Context.SaveChanges();
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
            Tool tool = Repo.Context.Tools.Find(id);
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
            Tool tool = Repo.Context.Tools.Find(id);
            Repo.Context.Tools.Remove(tool);
            Repo.Context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Repo.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
