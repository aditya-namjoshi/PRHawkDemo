# PRHawkDemo
PRHawk -- This is a C# .Net based MVC App and RESTful API using Web API.

To execute:  </br>
Method 1)  </br>
Run the application from Visual Studio IDE (.Net 4/ .Net 4.5)  </br>

  a) Replace the GitHub API credentials in the web.config file for the following key value pair:  
  add key="Password" value = ""  
  Update the value with the password. This file is available in the \PRHawkRestService folder  
  b) For the test cases to run, need to update the same in the app.config  
  add key="Password" value = ""  Update the value with the password. This file is available in the \PRHawkTests folder  </br>
 
The application has 3 parts:
1)  Standalone MVC app which can be accessed via the url http://localhost:{portno}/user/{username}. This will load a grid showing all the repositories for the user sorted by open pull requests.

2) Standalone Rest API which can be accessed via the url http://localhost:{portnpo}/api/username/{username} This will return the list of repositories for the user sorted by open pull requests. This will be Json/XML output based on the browser
 
3) Landing page to search a particular user via the url http://localhost:{portno}/Home . This will allow user to enter the userid to search. This consumes the Web API created in part 2. Click on GetRepositories will list all the repositories and sort them by open pullrequests.
PN: For the third part to work, need to update port number in the following file:
PRHawkRestService\Views\Home\Index.cshtml 
url: 'http://localhost:44683/api/username/' + $('#userid').val() + '?type=json', => Update the portno 44683 to the appropriate localhost port no.
                
Alternate method  
Method 2)  
There is a rar file that has the entire published website. /PRHawk.rar  
  a) Unzip and copy to a path  
  b) Update the web.config with credentials for github api  
  c) Deploy this to IIS or other server by creating a website pointing to this path. Name the website PRHawk  
  d) Navigate to the same urls as above without the port number.  
    Forex: for the first part from above, the link will be : http://localhost/PRHawk/user/{username} [Stand alone MVC]  
            for the second part from above, the link will be http://localhost/PRHawk/api/username/{username} [Web API Rest APIT]  
            for the third part from above, the link will be http://localhost/PRHawk/Home [MVC consuming the API created in part 2]  
            
  PN: Web.config file should have permissions for IIS_IUSRS to be able to run on IIS.
  
  
  
  
  


