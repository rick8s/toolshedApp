using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolshedApp.Models;

namespace ToolshedApp.Tests.Models
{
    [TestClass]
    public class ToolTests
    {
        [TestMethod]
        public void ToolEnsureICanCreateAnInstance()
        {
            Tool a_tool = new Tool();

            Assert.IsNotNull(a_tool);
        }

        [TestMethod]
        public void ToolEnsureAToolHasAllOfItsInfo()
        {
            //Arrange
            Tool a_tool = new Tool();

            // Act 
            a_tool.ToolId = 1;
            a_tool.Description = "My Content";
            a_tool.Owner = null; // Will need to define this later
            a_tool.Image = "https://google.com";
            a_tool.Category = "A Category";
            a_tool.Available = true;

            // Assert
            Assert.AreEqual(1, a_tool.ToolId);
            Assert.AreEqual("My Content", a_tool.Description);
            Assert.AreEqual(null, a_tool.Owner);
            Assert.AreEqual("https://google.com", a_tool.Image);
            Assert.AreEqual("A Category", a_tool.Category);
            Assert.AreEqual(true, a_tool.Available);
        }


    }
}
