using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Serialization;

namespace PRHawkRestService.Models
{
    public class Repository : IComparable<Repository>
    {
        public string Name { get; set; }
        public int OpenPullRequests { get; set; }
        public string Url { get; set; }

        public int CompareTo(Repository item)
        {
            return -1 * (this.OpenPullRequests.CompareTo(item.OpenPullRequests));
        }
    }

    public class RepositoryContractResolver : DefaultContractResolver
    {
        private Dictionary<string, string> PropertyMappings { get; set; }

        public RepositoryContractResolver()
        {
            this.PropertyMappings = new Dictionary<string, string> 
        {
            {"Name", "name"},
            {"OpenPullRequests", "OpenPullRequests"},
            {"Url", "svn_url"},
        };
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            string resolvedName = null;
            var resolved = this.PropertyMappings.TryGetValue(propertyName, out resolvedName);
            return (resolved) ? resolvedName : base.ResolvePropertyName(propertyName);
        }
    }
}