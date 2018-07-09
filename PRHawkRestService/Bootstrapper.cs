using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using PRHawkRestService.Controllers;
using PRHawkRestService.Services;
using PRHawkRestService.Models;
using Newtonsoft.Json;
using PRHawkRestService.WebSettings;

namespace PRHawkRestService
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IRepositoryService, RepositoryService>(
                new InjectionConstructor(new JsonSerializerSettings{ContractResolver = new RepositoryContractResolver()}, new WebConfigSettings()));
            container.RegisterType<IController, UserController>();
            return container;
        }
    }
}