using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PRHawkRestService.Models;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Web.Configuration;
using PRHawkRestService.WebSettings;
using System.Threading.Tasks;

namespace PRHawkRestService.Services
{
    public interface IRepositoryService
    {
        List<Repository> GetAllRepositoriesForUser(string username);
        void GetOpenPullRequestsForRepositories(List<Repository> repositories, string userName);
    }

    public class RepositoryService : IRepositoryService
    {
        private string UserAgent;
        private string Authentication;
        private string userBaseUrl;
        private string repoBaseUrl;
        private string userId;
        private string password;

        private static HttpClient client = new HttpClient();
       
        private JsonSerializerSettings settings;

        public RepositoryService(JsonSerializerSettings settings, WebConfigSettings webSettings) 
        {
            this.settings = settings;
            UserAgent = webSettings.GetUserAgent();
            Authentication = webSettings.GetAuthentication();
            userBaseUrl = webSettings.GetUserBaseUrl();
            repoBaseUrl = webSettings.GetRepoBaseUrl();
            userId = webSettings.GetUserName();
            password = webSettings.GetPassword();
            SetHeaders();
        }

        // Gets all repositories for a particular user name
        public virtual List<Repository> GetAllRepositoriesForUser(string userName)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            
            List<Repository> result = new List<Repository>();

            try
            {
                var uri = BuildRepoListUrl(userName);

                var response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    //Parse JSON to object
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<List<Repository>>(jsonResponse, settings);

                    // Check for pages
                    var nextUri = GetNextUri(response);
                    while (!String.IsNullOrEmpty(nextUri))
                    {
                        var pagedResponse = client.GetAsync(nextUri).Result;
                        if (pagedResponse.IsSuccessStatusCode)
                        {
                            //Parse JSON to object
                            var pagedJsonResponse = pagedResponse.Content.ReadAsStringAsync().Result;
                            result.AddRange(JsonConvert.DeserializeObject<List<Repository>>(pagedJsonResponse, settings));

                            // Check for pages
                            nextUri = GetNextUri(pagedResponse);
                        }
                        else
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                // Can also throw error  if response status code i snot success and handle here
            }

            return result;
        }
        
        // Loops through each repository for a user and updates the total open pull request
        public virtual void GetOpenPullRequestsForRepositories(List<Repository> repositories, string userName)
        {
            foreach(var repo in repositories)
            {
                try
                {
                    // Requests for 1 pull request detail at a time and uses the last page url to get the total count
                    // Default return is only "Open" pull requests
                    var uri = BuildPullReqUrl(userName, repo.Name);

                    var response = client.GetAsync(uri).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        repo.OpenPullRequests = GetCountOfOpenPullRequests(response);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception here
                    // Handle if success code is error as well
                }
            }

            repositories.Sort(); // Sort descending
        }

        private string BuildRepoListUrl(string userName)
        {
            return userBaseUrl + userName + "/repos?per_page=100";
        }

        private string BuildPullReqUrl(string userName, string repoName)
        {
            return repoBaseUrl + userName + '/' + repoName + "/pulls?per_page=1";
        }

        // Set sthe outgoing headers required by the git api
        private void SetHeaders()
        {
            // Headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            client.DefaultRequestHeaders.Add("Authorization", Authentication + " " + GetToken());
        }

        // Encodes username and password
        private string GetToken()
        {
            var combToken = userId + ":" + password;
            var bytes = System.Text.Encoding.UTF8.GetBytes(combToken);
            return System.Convert.ToBase64String(bytes);
        }

        // Returns next url if paged response from git api
        private string GetNextUri(HttpResponseMessage response)
        {
            IEnumerable<String> linkHeader;
            response.Headers.TryGetValues("Link", out linkHeader); // Contains paged urls
            string uri = "";

            if (linkHeader != null && linkHeader.Count() > 0)
            {
                var links = linkHeader.First().ToString();
                var splitLinks = links.Split(',');
                foreach (var link in splitLinks) // Search all the links to check the next url present
                {
                    var index = link.IndexOf(" rel=\"next\"");
                    if (index == -1)
                        continue;

                    var next = link.Split(';');
                    uri = next[0].Trim(new char[] { '<', '>', ' ' });
                }
            }
            return uri;
        }

        // Returns count of total open pull requests by looking at last page url
        private int GetCountOfOpenPullRequests(HttpResponseMessage response)
        {
            int count = 0;
            IEnumerable<String> linkHeader;
            response.Headers.TryGetValues("Link", out linkHeader);

            if (linkHeader != null && linkHeader.Count() > 0)
            {
                var links = linkHeader.First().ToString();
                var splitLinks = links.Split(',');
                var last = splitLinks[1].Split(';'); // Second link is the last page link
                var indexOfCount = last[0].IndexOf("&page="); // Get the index of the last page number
                var lastPage = last[0][indexOfCount + 6];
                count = (int)Char.GetNumericValue(lastPage); // Last page number is the total open pull requests
            }

            return count;
        }
    }
}