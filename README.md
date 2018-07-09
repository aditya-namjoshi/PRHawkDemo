# PRHawkDemo
PRHawk -- This is a C# .Net based MVC App and RESTful API using Web API.

To execute:
1) Run the application from Visual Studio IDE (.Net 4/ .Net 4.5)
  a) Replace the GitHub API credentials in the web.config file for the following key value pair:
  <add key="Password" value = ""> Update the value with the password. This file is available in the \PRHawkRestService folder
  b) For the test cases to run, need to update the same in the app.config
  <add key="Password" value = ""> Update the value with the password. This file is available in the \PRHawkTests folder
 
The application has 3 parts:
1) Standalone Rest API which can be accessed via the url http://localhost:{portnpo}/api/username/{user} This will return the list of repositories for the user sorted by open pull requests. This will be Json/XML output based on the browser
 
2) Landing page to search a particular user via the url http://localhost:44683/Home . This will allow user to enter the userid to search.
Click on GetRepositories will list all the repositories and sort them by open pullrequests

3) Standalone MVC app which can be accessed via the url http://localhost:44683/user/{username}. This will load a grid showing all the repositories for the user sorted by open pull requests.

Alternate method
2) There is a zip file that has the entire published website.
  a) Unzip and copy to a path
  b) Deploy this to IIS or other server by creating a website pointing to this path.
  
  PN: Web.config file should have permissions for IIS_IUSRS to be able to run on IIS.


