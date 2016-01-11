using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;



namespace ToolshedApp.Models
{
    public class ToolshedRepository
    {
        private ToolshedContext _context;
        public ToolshedContext Context { get { return _context; } }

        public ToolshedRepository()
        {
            _context = new ToolshedContext();
        }
        public ToolshedRepository(ToolshedContext a_context)
        {
            _context = a_context;
        }

        public List<ToolshedUser> GetAllUsers()
        {
            var query = from users in _context.ToolshedUsers select users;
            return query.ToList();
        }

        public ToolshedUser GetUserByUserName(string username)
        {

            var query = from user in _context.ToolshedUsers where user.UserName == username select user;

            return query.SingleOrDefault();
        }

        public bool IsUserNameAvailable(string username)
        {
            bool available = false;
            try
            {
                ToolshedUser some_user = GetUserByUserName(username);
                if (some_user == null)
                {
                    available = true;
                }
            }
            catch (InvalidOperationException) { }

            return available;
        }

        public List<ToolshedUser> SearchByUserName(string username)
        {
            var query = from user in _context.ToolshedUsers select user;
            List<ToolshedUser> found_users = query.Where(user => user.UserName.Contains(username)).ToList();
            found_users.Sort();
            return found_users;
        }

        public List<ToolshedUser> SearchByName(string search_term)
        {
            var query = from user in _context.ToolshedUsers select user;
            List<ToolshedUser> found_users = query.Where(user => Regex.IsMatch(user.FirstName, search_term, RegexOptions.IgnoreCase) || Regex.IsMatch(user.LastName, search_term, RegexOptions.IgnoreCase)).ToList();
            found_users.Sort();
            return found_users;
        }

        public List<Tool> GetAllTools()
        {
            // SQL: select * from Tools;
            var query = from tool in _context.Tools select tool;
            List<Tool> found_tools = query.ToList();
            found_tools.Sort();
            return found_tools;
        }

        public bool CreateTool(ToolshedUser me, string name, string category, string descrip, string pic)
        {

            Tool a_tool = new Tool { Owner = me, Name = name, Category = category, Description = descrip, Image = pic, Available = true };
            bool is_added = true;
            try
            {
                Tool added_tool = _context.Tools.Add(a_tool);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                is_added = false;
            }
            return is_added;
        }

        public bool CreateToolshedUser(ApplicationUser app_user, string first, string last, string userName, string phone, string street)
        {
           // bool user_name_is_available = this.IsUserNameAvailable(userName);
           // if (user_name_is_available)
           // {
                ToolshedUser a_user = new ToolshedUser { RealUser = app_user, FirstName = first, LastName = last, UserName = userName, Phone = phone, Street = street };
                bool is_added = true;
                try
                {
                    ToolshedUser added_user = _context.ToolshedUsers.Add(a_user);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    is_added = false;
                }

                return is_added;
           // }
         
        }

        public List<Tool> GetAvailableTools()
        {
            var query = from tool in _context.Tools select tool;
            List<Tool> found_available = query.Where(tool => tool.Available == true).ToList();

            List<Tool> found_available_sorted = found_available.OrderBy(tool => tool.Available).ToList();
            return found_available_sorted;
        }

        public List<Tool> GetOthersAvailableTools(ToolshedUser user)
        {

            if (user != null)
            {
                var query = from u in _context.ToolshedUsers where u.UserId == user.UserId select u;
                var tool_query = from t in _context.Tools where t.Available == true && t.Owner.UserId != user.UserId select t;
                ToolshedUser found_users = query.Single<ToolshedUser>();
                List<Tool> these_users_tools = tool_query.ToList();
                if (found_users == null)
                {
                    return new List<Tool>();
                }
                if (tool_query == null)
                {
                    return new List<Tool>();
                }
                else
                {
                    List<Tool> found_available_sorted = these_users_tools.OrderBy(tool => tool.Owner).ToList();
                    return found_available_sorted;
                }

            }
            else
            {
                return new List<Tool>();
            }




            //var query = from tool in _context.Tools select tool;
            //var tool_query = from t in _context.Tools where t.Owner.UserId != user.UserId select t;
           // List<Tool> found_available = query.Where(tool => tool.Available == true).ToList();

           // List<Tool> found_available_sorted = found_available.OrderBy(tool => tool.Available).ToList();
            //return found_available_sorted;
        }















        public List<Tool> GetUserTools(ToolshedUser user)
        {

            if (user != null)
            {
                var query = from u in _context.ToolshedUsers where u.UserId == user.UserId select u;
                var tool_query = from t in _context.Tools where t.Owner.UserId == user.UserId select t;
                ToolshedUser found_user = query.Single<ToolshedUser>();
                List<Tool> this_users_tools = tool_query.ToList();
                if (found_user == null)
                {                
                    return new List<Tool>();
                }
                if (tool_query == null)
                {
                    return new List<Tool>();
                }
                else
                {
                    return this_users_tools;
                }
               
            }
            else
            {
                return new List<Tool>();
            }
        }

        public List<Tool> SearchByToolName(string search_term)
        {
            var query = from tool in _context.Tools select tool;
            List<Tool> found_tools = query.Where(tool => Regex.IsMatch(tool.Name, search_term, RegexOptions.IgnoreCase)).ToList();
            found_tools.Sort();
            return found_tools;
        }

        public List<Tool> SearchByToolCategory(string search_term)
        {
            var query = from tool in _context.Tools select tool;
            List<Tool> found_tools = query.Where(tool => tool.Category.Contains(search_term)).ToList();
            found_tools.Sort();
            return found_tools;
        }

        public bool AddNewUser(ApplicationUser user)
        {
            ToolshedUser new_user = new ToolshedUser { RealUser = user};
            bool is_added = true;
            try
            {
                ToolshedUser added_user = _context.ToolshedUsers.Add(new_user);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                is_added = false;
            }
            return is_added;
        }
    }
}