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

namespace PRHawkTests
{
    [TestClass]
    public class RepositoryServiceTests
    {
        Mock<WebConfigSettings> webSettings;

        public RepositoryServiceTests()
        {
            webSettings = new Mock<WebConfigSettings>(); 
            webSettings.Setup(o => o.GetUserAgent()).Returns("PRHawkDemo");
            webSettings.Setup(o => o.GetAuthentication()).Returns("Basic");
            webSettings.Setup(o => o.GetUserBaseUrl()).Returns("https://api.github.com/users/");
            webSettings.Setup(o => o.GetRepoBaseUrl()).Returns("https://api.github.com/repos/");
            webSettings.Setup(o => o.GetUserName()).Returns("snkirklandinterview");
            webSettings.Setup(o => o.GetPassword()).Returns("07c76ed5f66329252f38f43a8472a8f741047271");
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
