using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PRHawkRestService.Models;
using PRHawkRestService.Services;
using Newtonsoft.Json;
using PRHawkRestService.WebSettings;

namespace PRHawkRestService.Controllers
{
    public class UsernameController : ApiController
    {
        private IRepositoryService _repositoryService;

        // Default constructor
        public UsernameController()
        {
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.ContractResolver = new RepositoryContractResolver();
            var webSettings = new WebConfigSettings();
            _repositoryService = new RepositoryService(jsonSettings, webSettings);
        }

        // Constructor for unit testing and for dependency injection
        public UsernameController(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        // Return list of repositories for a user sorted on open pull requests
        // /api/username/{id}
        public IEnumerable<Repository> Get(string id)
        {
            var repositories = _repositoryService.GetAllRepositoriesForUser(id);
            _repositoryService.GetOpenPullRequestsForRepositories(repositories, id);
            return repositories;
        }
    }
}
