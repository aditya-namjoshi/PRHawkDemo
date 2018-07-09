using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PRHawkRestService.Controllers;
using PRHawkRestService.Services;
using PRHawkRestService.Models;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;

namespace PRHawkTests
{
    [TestClass]
    public class UsernameControllerTest
    {
        [TestMethod]
        public void Should_Call_AllFunctions()
        {
            // Arrange
            var fakeRepoServ = new Mock<IRepositoryService>();
            fakeRepoServ.Setup(o => o.GetAllRepositoriesForUser(It.IsAny<string>())).Returns(new List<Repository>());
            UsernameController controller = new UsernameController(fakeRepoServ.Object);

            // Act
            controller.Get(It.IsAny<string>());

            // Assert
            fakeRepoServ.Verify(o => o.GetAllRepositoriesForUser(It.IsAny<string>()), Times.Once);
            fakeRepoServ.Verify(o => o.GetOpenPullRequestsForRepositories(new List<Repository>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Should_ReturnRepositories_WhenServiceReturnsRepos()
        {
            // Arrange
            var fakeRepoServ = new Mock<IRepositoryService>();

            var repositories = new List<Repository> 
            { new Repository { Name = "Repo1", OpenPullRequests = 1, Url = "Test1" }, 
                new Repository { Name = "Repo2", OpenPullRequests = 2, Url = "Test2" } };

            fakeRepoServ.Setup(o => o.GetAllRepositoriesForUser(It.IsAny<string>())).Returns(repositories);
            fakeRepoServ.Setup(o => o.GetOpenPullRequestsForRepositories(repositories, It.IsAny<string>()));
            var controller = new UsernameController(fakeRepoServ.Object);

            // Act
            var retRepos = controller.Get(It.IsAny<string>());

            // Assert
            Assert.IsNotNull(retRepos);
            Assert.AreEqual(2, retRepos.Count());
            Assert.AreEqual("Repo1", retRepos.ElementAt(0).Name);
            Assert.AreEqual(1, retRepos.ElementAt(0).OpenPullRequests);
            Assert.AreEqual("Test1", retRepos.ElementAt(0).Url);

            Assert.AreEqual("Repo2", retRepos.ElementAt(1).Name);
            Assert.AreEqual(2, retRepos.ElementAt(1).OpenPullRequests);
            Assert.AreEqual("Test2", retRepos.ElementAt(1).Url);
        }

        [TestMethod]
        public void Should_ReturnEmpty_WhenServiceDoesNotReturnRepos()
        {
            // Arrange
            var fakeRepoServ = new Mock<IRepositoryService>();

            var repositories = new List<Repository>();

            fakeRepoServ.Setup(o => o.GetAllRepositoriesForUser(It.IsAny<string>())).Returns(repositories);
            fakeRepoServ.Setup(o => o.GetOpenPullRequestsForRepositories(repositories, It.IsAny<string>()));
            var controller = new UsernameController(fakeRepoServ.Object);

            // Act
            var retRepos = controller.Get(It.IsAny<string>());

            // Assert
            Assert.IsNotNull(retRepos);
            Assert.AreEqual(0, retRepos.Count());
        }
    }
}
