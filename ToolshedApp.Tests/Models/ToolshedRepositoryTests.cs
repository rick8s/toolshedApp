using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolshedApp.Models;
using System.Collections.Generic;
using Moq;
using System.Data.Entity;
using System.Linq;

namespace ToolshedApp.Tests.Models
{
    [TestClass]
    public class ToolshedRepositoryTests 
    {
        private Mock<ToolshedContext> mock_context;
        private Mock<DbSet<ToolshedUser>> mock_set;
        private Mock<DbSet<Tool>> mock_tool_set;
        private Mock<DbSet<ToolReserve>> mock_reserved_tool_set;

        private ToolshedRepository repository;

        private void ConnectMocksToDataStore(IEnumerable<ToolshedUser> data_store)
        {
            var data_source = data_store.AsQueryable<ToolshedUser>();

            mock_set.As<IQueryable<ToolshedUser>>().Setup(data => data.Provider).Returns(data_source.Provider);
            mock_set.As<IQueryable<ToolshedUser>>().Setup(data => data.Expression).Returns(data_source.Expression);
            mock_set.As<IQueryable<ToolshedUser>>().Setup(data => data.ElementType).Returns(data_source.ElementType);
            mock_set.As<IQueryable<ToolshedUser>>().Setup(data => data.GetEnumerator()).Returns(data_source.GetEnumerator());

            // This is Stubbing the ToolshedUsers property getter
            mock_context.Setup(a => a.ToolshedUsers).Returns(mock_set.Object);
        }

        private void ConnectMocksToDataStore(IEnumerable<Tool> data_store)
        {
            var data_source = data_store.AsQueryable<Tool>();

            mock_tool_set.As<IQueryable<Tool>>().Setup(data => data.Provider).Returns(data_source.Provider);
            mock_tool_set.As<IQueryable<Tool>>().Setup(data => data.Expression).Returns(data_source.Expression);
            mock_tool_set.As<IQueryable<Tool>>().Setup(data => data.ElementType).Returns(data_source.ElementType);
            mock_tool_set.As<IQueryable<Tool>>().Setup(data => data.GetEnumerator()).Returns(data_source.GetEnumerator());

            // This is Stubbing the Tools property getter
            mock_context.Setup(a => a.Tools).Returns(mock_tool_set.Object);
        }

        private void ConnectMocksToDataStore(IEnumerable<ToolReserve> data_store)
        {
            var data_source = data_store.AsQueryable<ToolReserve>();

            mock_reserved_tool_set.As<IQueryable<ToolReserve>>().Setup(data => data.Provider).Returns(data_source.Provider);
            mock_reserved_tool_set.As<IQueryable<ToolReserve>>().Setup(data => data.Expression).Returns(data_source.Expression);
            mock_reserved_tool_set.As<IQueryable<ToolReserve>>().Setup(data => data.ElementType).Returns(data_source.ElementType);
            mock_reserved_tool_set.As<IQueryable<ToolReserve>>().Setup(data => data.GetEnumerator()).Returns(data_source.GetEnumerator());

            // This is Stubbing the Tools property getter
            mock_context.Setup(a => a.Reserved).Returns(mock_reserved_tool_set.Object);
        }

    

        [TestInitialize]
        public void Initialize()
        {
            mock_context = new Mock<ToolshedContext>();
            mock_set = new Mock<DbSet<ToolshedUser>>();
            mock_tool_set = new Mock<DbSet<Tool>>();
           // mock_reserved_tool_set = new Mock<DbSet<ToolReserve>>();
            repository = new ToolshedRepository(mock_context.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            mock_context = null;
            mock_set = null;
            mock_tool_set = null;
           // mock_reserved_tool_set = null;
            repository = null;
        }

        [TestMethod]
        public void ToolshedContextEnsureICanCreateInstance()
        {
            ToolshedContext context = new ToolshedContext();
            Assert.IsNotNull(context);
        }

        [TestMethod]
        public void ToolshedRepositoryEnsureICanCreateInstance()
        {
            ToolshedRepository repository = new ToolshedRepository();
            Assert.IsNotNull(repository);
        }

        [TestMethod]
        public void ToolshedRepositoryEnsureICanGetAllUsers()
        {
            //Arrange
            var expected = new List<ToolshedUser>
            {
                new ToolshedUser { UserName = "garagedude"  },
                new ToolshedUser { UserName = "toolman" },
                new ToolshedUser { UserName = "tooldaddy" }
            };

            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);

            //Act
            var actual = repository.GetAllUsers();

            //Assert
            Assert.AreEqual("garagedude", actual.First().UserName);
            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void ToolshedRepositoryEnsureIHaveAContext()
        {
            //Act
            var actual = repository.Context;

            //Assert
            Assert.IsInstanceOfType(actual, typeof(ToolshedContext));
        }

        [TestMethod]
        public void ToolshedRepositoryEnsureICanGenUserByUserName()
        {
            // Arrange
            var expected = new List<ToolshedUser>
            {
                new ToolshedUser {UserName = "garagedude" },
                new ToolshedUser { UserName = "shoprat"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string username = "shoprat";
            ToolshedUser actual_user = repository.GetUserByUserName(username);
            // Assert
            Assert.AreEqual("shoprat", actual_user.UserName);
        }

        [TestMethod]
        public void ToolshedRepositoryGetUserByUserNameUserDoesNotExist()
        {
            // Arrange
            var expected = new List<ToolshedUser>
            {
                new ToolshedUser {UserName = "garagedude" },
                new ToolshedUser { UserName = "shoprat"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string username = "bogus";
            ToolshedUser actual_user = repository.GetUserByUserName(username);
            // Assert
            Assert.IsNull(actual_user);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToolshedRepositoryGetUserByUserNameFailsMultipleUsers()
        {
            // Arrange
            var expected = new List<ToolshedUser>
            {
                new ToolshedUser {UserName = "garagedude" },
                new ToolshedUser { UserName = "garagedude"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string username = "garagedude";
            ToolshedUser actual_user = repository.GetUserByUserName(username);
            // Assert
        }

        [TestMethod]
        public void ToolshedRepositoryEnsureUserNameIsAvailable()
        {
            // Arrange
            var expected = new List<ToolshedUser>
            {
                new ToolshedUser {UserName = "garagedude" },
                new ToolshedUser { UserName = "shoprat"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string username = "bogus";
            bool is_available = repository.IsUserNameAvailable(username);
            // Assert
            Assert.IsTrue(is_available);
        }

        [TestMethod]
        public void ToolshedRepositoryEnsureUserNameIsNotAvailable()
        {
            // Arrange
            var expected = new List<ToolshedUser>
            {
                new ToolshedUser {UserName = "garagedude" },
                new ToolshedUser { UserName = "shoprat"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string username = "garagedude";
            bool is_available = repository.IsUserNameAvailable(username);
            // Assert
            Assert.IsFalse(is_available);

        }

        [TestMethod]
        public void ToolshedRepositoryEnsureUserNameIsNotAvailableMultipleUsers()
        {
            // Arrange
            var expected = new List<ToolshedUser>
            {
                new ToolshedUser {UserName = "garagedude" },
                new ToolshedUser { UserName = "garagedude"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string username = "garagedude";
            bool is_available = repository.IsUserNameAvailable(username);
            // Assert
            Assert.IsFalse(is_available);
        }

        [TestMethod]
        public void ToolshedRepositoryEnsureICanSearchByUserName()
        {
            // Arrange
            var expected = new List<ToolshedUser>
            {
                new ToolshedUser { UserName = "garagedude" },
                new ToolshedUser { UserName = "shoprat"},
                new ToolshedUser { UserName = "toolman" },
                new ToolshedUser { UserName = "tooldaddy"}

            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string username = "tool";
            List<ToolshedUser> expected_users = new List<ToolshedUser>
            {
                new ToolshedUser { UserName = "tooldaddy"},
                new ToolshedUser { UserName = "toolman" }
            };
            List<ToolshedUser> actual_users = repository.SearchByUserName(username);

            // Assert
            Assert.AreEqual(expected_users[0].UserName, actual_users[0].UserName);
            Assert.AreEqual(expected_users[1].UserName, actual_users[1].UserName);
        }

        [TestMethod]
        public void ToolshedRepositoryEnsureICanSearchByName()
        {
            // Arrange
            var expected = new List<ToolshedUser>
            {
                new ToolshedUser { UserName = "garagedude", FirstName = "Sam", LastName = "Sneed" },
                new ToolshedUser { UserName = "shoprat", FirstName = "Pete", LastName = "Sampras"},
                new ToolshedUser { UserName = "tooldaddy", FirstName = "Harley", LastName = "Davidson" },
                new ToolshedUser { UserName = "toolman", FirstName = "Samuel", LastName = "Adams"}

            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string search_term = "sam";
            List<ToolshedUser> expected_users = new List<ToolshedUser>
            {
                new ToolshedUser { UserName = "garagedude", FirstName = "Sam", LastName = "Sneed" },
                new ToolshedUser { UserName = "shoprat", FirstName = "Pete", LastName = "Sampras"},
                new ToolshedUser { UserName = "toolman", FirstName = "Samuel", LastName = "Adams"}
            };
            List<ToolshedUser> actual_users = repository.SearchByName(search_term);

            // Assert
            Assert.AreEqual(expected_users[0].UserName, actual_users[0].UserName);
            Assert.AreEqual(expected_users[1].UserName, actual_users[1].UserName);
            Assert.AreEqual(expected_users[2].UserName, actual_users[2].UserName);
        }

        [TestMethod]
        public void ToolshedRepositoryEnsureICanGetAllTools()
        {
            // Arrange

            List<Tool> expected_tools = new List<Tool>
            {
                new Tool { Name = "Table Saw" },
                new Tool { Name = "Cordless Drill"},
                new Tool { Name = "Nail Gun" }
            };
            mock_tool_set.Object.AddRange(expected_tools);
            ConnectMocksToDataStore(expected_tools);
            // Act
            List<Tool> actual_tools = repository.GetAllTools();
            expected_tools.Sort();
            actual_tools.Sort();

            // Assert
            Assert.AreEqual(expected_tools[0].Name, actual_tools[0].Name);
            Assert.AreEqual(expected_tools[1].Name, actual_tools[1].Name);
            Assert.AreEqual(expected_tools[2].Name, actual_tools[2].Name);
            Assert.AreEqual("Cordless Drill", actual_tools[0].Name); // Just to check ourselves
        }

        [TestMethod]
        public void ToolshedRepositoryEnsureICanAddATool()
        {
            // Arrange

            List<Tool> expected_tools = new List<Tool>(); // This is our database
            ConnectMocksToDataStore(expected_tools);
            ToolshedUser toolshed_user1 = new ToolshedUser { UserName = "toolman" };
            string name = "Compressor";
            string category = "Power Tool";
            string descrip = "10gal 1.25hp";
            string pic = "https://google.com";

            mock_tool_set.Setup(t => t.Add(It.IsAny<Tool>())).Callback((Tool s) => expected_tools.Add(s));
            // Act
            bool successful = repository.CreateTool(toolshed_user1, name, category, descrip, pic);

            // Assert
            Assert.AreEqual(1, repository.GetAllTools().Count);
            // Should this return true?
            Assert.IsTrue(successful);
        }

        [TestMethod]
        public void ToolRepositoryEnsureBorrowedToolIsUnavailable()
        {
            //Arrange

            List<Tool> list_of_tools = new List<Tool>
            {
                new Tool { ToolId = 1, Name = "cordless drill", Available = true },
                new Tool { ToolId = 2, Name = "table saw", Available = true },
                new Tool { ToolId = 3, Name = "band saw", Available = false },
                new Tool { ToolId = 4, Name = "compressor", Available = true }
            };

            ConnectMocksToDataStore(list_of_tools);

            //Act
            List<Tool> available_tools = repository.GetAvailableTools();

            //Assert

            Assert.AreEqual(list_of_tools[0].ToolId, available_tools[0].ToolId);
            Assert.AreEqual(list_of_tools[1].ToolId, available_tools[1].ToolId);
            Assert.AreEqual(list_of_tools[3].ToolId, available_tools[2].ToolId);
        }
    }
}
