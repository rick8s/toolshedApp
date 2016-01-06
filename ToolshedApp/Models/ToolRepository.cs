using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ToolshedApp.Models
{
    public class ToolRepository
    {
        private ToolshedContext _context;
        public ToolshedContext Context { get { return _context; } }

        public ToolRepository()
        {
            _context = new ToolshedContext();
        }
        public ToolRepository(ToolshedContext a_context)
        {
            _context = a_context;
        }
    }
}