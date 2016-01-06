using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolshedApp.Models;

namespace ToolshedApp.Tests.Models
{
    [TestClass]
    public class ToolRepositoryTests
    {
        [TestMethod]
        public void ToolshedContextEnsureICanCreateInstance()
        {
            ToolshedContext context = new ToolshedContext();
            Assert.IsNotNull(context);
        }

        [TestMethod]
        public void ToolshedRepositoryEnsureICanCreateInstance()
        {
            ToolRepository repository = new ToolRepository();
            Assert.IsNotNull(repository);
        }
    }
}
