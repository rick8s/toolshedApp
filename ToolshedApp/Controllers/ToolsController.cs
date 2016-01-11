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
using System.Text.RegularExpressions;



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
        public ActionResult Index(string search_string)
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
                if(successful)
                {
                    me = Repo.GetAllUsers().Where(u => u.RealUser.Id == user_id).Single();
                }
            }

            List<Tool> others_tools = Repo.GetOthersAvailableTools(me);
            var search_tools = from t in others_tools where t.Owner.UserId != me.UserId select t;
            
            if(!String.IsNullOrEmpty(search_string))
            {
                search_tools = search_tools.Where(t => Regex.IsMatch(t.Name, search_string, RegexOptions.IgnoreCase) || Regex.IsMatch(t.Owner.FirstName, search_string, RegexOptions.IgnoreCase) || t.Description.Contains(search_string) || Regex.IsMatch(t.Category, search_string, RegexOptions.IgnoreCase));
                
                return View(search_tools);               
            }
            else
            {
                return View(others_tools);
            }
        }

        // GET: MY Tools
        [Authorize]
        public ActionResult MyTools()
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

            List<Tool> my_tools = Repo.GetUserTools(me);
            return View(my_tools);
        }

        // GET: Tools I borrowed
        [Authorize]
        public ActionResult MyBorrowed()
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

            List<Tool> my_tools = Repo.GetListOfToolsThisUserBorrowed(me);
            return View(my_tools);
        }

        // GET: Tools I loaned
        [Authorize]
        public ActionResult MyLoaned()
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

            List<Tool> my_tools = Repo.GetListOfToolsThisUserLoaned(me);
            return View(my_tools);
        }


        // GET: All Borrowed Tools
        [Authorize]
        public ActionResult AllLoaned()
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

            List<Tool> all_borrowed_tools = Repo.GetAllBorrowedTools();
            return View(all_borrowed_tools);
        }

        // GET: Tools/Borrow
        public ActionResult Borrow(int? id)
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
                if (successful)
                {
                    me = Repo.GetAllUsers().Where(u => u.RealUser.Id == user_id).Single();
                }
            }

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
            tool.Borrowed = true;
            tool.Borrower = me;
            Repo.Context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Tools/Return
        public ActionResult Return(int? id)
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
            tool.Available = true;
            tool.Borrowed = false;
            Repo.Context.SaveChanges();
            return RedirectToAction("Index");
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
