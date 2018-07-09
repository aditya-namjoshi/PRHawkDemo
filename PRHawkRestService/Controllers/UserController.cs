using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using PRHawkRestService.Models;
using PRHawkRestService.Services;
using Newtonsoft.Json;

namespace PRHawkRestService.Controllers
{
    // Controller that passes list of repositories sorted by open pull requests for a partiuclar user to the view
    public class UserController : Controller
    {
        private IRepositoryService _repositoryService;

        public UserController(IRepositoryService repositoryService)
        {
           _repositoryService = repositoryService;
        }

        // user/{username}
        public ActionResult Get(string username)
        {
            var repositories = _repositoryService.GetAllRepositoriesForUser(username);
            _repositoryService.GetOpenPullRequestsForRepositories(repositories, username);
            return View(repositories);
        }
    }
}
