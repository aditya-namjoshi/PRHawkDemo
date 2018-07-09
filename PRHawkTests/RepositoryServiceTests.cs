using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using PRHawkRestService.Services;
using PRHawkRestService.Controllers;
using PRHawkRestService.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using PRHawkRestService.WebSettings;
using System.Configuration;

namespace PRHawkTests
{
    [TestClass]
    public class RepositoryServiceTests
    {
        Mock<WebConfigSettings> webSettings;

        public RepositoryServiceTests()
        {
            webSettings = new Mock<WebConfigSettings>();
            webSettings.Setup(o => o.GetUserAgent()).Returns(ConfigurationManager.AppSettings["UserAgent"]);
            webSettings.Setup(o => o.GetAuthentication()).Returns(ConfigurationManager.AppSettings["Authentication"]);
            webSettings.Setup(o => o.GetUserBaseUrl()).Returns(ConfigurationManager.AppSettings["UserBaseUrl"]);
            webSettings.Setup(o => o.GetRepoBaseUrl()).Returns(ConfigurationManager.AppSettings["RepoBaseUrl"]);
            webSettings.Setup(o => o.GetUserName()).Returns(ConfigurationManager.AppSettings["UserName"]);
            webSettings.Setup(o => o.GetPassword()).Returns(ConfigurationManager.AppSettings["Password"]);
        }

        [TestMethod]
        public void ShouldReturn_EmptyRepositories_For_InValidUser()
        {
            // Arrange
            var repoService = new RepositoryService(new JsonSerializerSettings(), webSettings.Object);
            var userName = "1234";

            // Act
            var repos = repoService.GetAllRepositoriesForUser(userName);

            // Assert
            Assert.IsNotNull(repos);
            Assert.IsTrue(repos.Count == 0);
        }

        [TestMethod]
        public void ShouldReturn_Repositories_For_ValidUser()
        {
            // Arrange
            var repoService = new RepositoryService(new JsonSerializerSettings(), webSettings.Object);
            var userName = "adam-golab";

            // Act
            var repos = repoService.GetAllRepositoriesForUser(userName);

            // Assert
            Assert.IsNotNull(repos);
            Assert.IsTrue(repos.Count > 0);
        }

        [TestMethod]
        public void ShouldReturnSortedRepositoriesForValidUser()
        {
            // Arrange
            var repoService = new RepositoryService(new JsonSerializerSettings(), webSettings.Object);
            var userName = "adam-golab";
            var repos = repoService.GetAllRepositoriesForUser(userName);

            // Act
            repoService.GetOpenPullRequestsForRepositories(repos,userName);

            var sortedRepos = repos.OrderByDescending(o => o.OpenPullRequests).ToList();

            // Assert
            Assert.IsNotNull(repos);
            Assert.IsTrue(repos.Count > 0);
            Assert.IsTrue(sortedRepos.SequenceEqual(repos));
        }
    }
}
