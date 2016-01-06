using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolshedApp.Models;
using System.Collections.Generic;

namespace ToolshedApp.Tests.Models
{
    [TestClass]
    public class ToolshedUserTests
    {
        [TestMethod]
        public void ToolshedUserEnsureICanCreateInstance()
        {
            ToolshedUser a_user = new ToolshedUser();
            Assert.IsNotNull(a_user);
        }

        [TestMethod]
        public void ToolshedUserEnsureJitterUserHasAllTheThings()
        {
            // Arrange
            ToolshedUser a_user = new ToolshedUser();

            a_user.UserId = 1;
            a_user.FirstName = "Jim";
            a_user.LastName = "Beam";
            a_user.Phone = "111-222-3333";
            a_user.Street = "Anystreet Blvd";

            // Assert 
            Assert.AreEqual(1, a_user.UserId);
            Assert.AreEqual("111-222-3333", a_user.Phone);
            Assert.AreEqual("Jim", a_user.FirstName);
            Assert.AreEqual("Beam", a_user.LastName);
            Assert.AreEqual("Anystreet Blvd", a_user.Street);
        }
    }
}
