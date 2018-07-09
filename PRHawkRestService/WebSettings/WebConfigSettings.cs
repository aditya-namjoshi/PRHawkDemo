using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace PRHawkRestService.WebSettings
{
    public class WebConfigSettings
    {
        public virtual string GetUserAgent()
        {
            return WebConfigurationManager.AppSettings["UserAgent"];
        }

        public virtual string GetAuthentication()
        {
            return WebConfigurationManager.AppSettings["Authentication"];
        }

        public virtual string GetUserBaseUrl()
        {
            return WebConfigurationManager.AppSettings["userBaseUrl"];
        }

        public virtual string GetRepoBaseUrl()
        {
            return WebConfigurationManager.AppSettings["repoBaseUrl"];
        }

        public virtual string GetUserName()
        {
            return WebConfigurationManager.AppSettings["userName"];
        }

        public virtual string GetPassword()
        {
            return WebConfigurationManager.AppSettings["password"];
        }
    }
}