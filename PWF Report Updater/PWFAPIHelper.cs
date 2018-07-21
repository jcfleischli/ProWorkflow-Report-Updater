using System;
using RestSharp;

namespace PWF_Report_Updater
{
    class PWFAPIHelper
    {
        // Removed for security
        private const string BASEURI  = "";
        private const string USERNAME = "";
        private const string PASSWORD = "";
        private const string APIKEY   = "";

        public string ProjectsReply = "";

        public JsonResultProjects GetProjects()
        {
            try
            {
                RestSharp.RestResponse response = DoRequest("projects", "project");
                ProjectsReply = response.Content.ToString();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResultProjects>(ProjectsReply);
            }
            catch(Exception e)
            {
                SlackClient client = new SlackClient("");
                client.PostMessage("PWF API Tool Error: " + e.ToString(), "PWF API Admin", "C9X3YGW5B");

                return null;
            }

        }

        public JsonResultTasks GetTasks(string projectId)
        {
            try { 
                RestSharp.RestResponse response = DoRequest("projects/" + projectId + "/tasks?status=all", "task");
                ProjectsReply = response.Content.ToString();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResultTasks>(ProjectsReply);
            }
            catch(Exception e)
            {
                SlackClient client = new SlackClient("");
                client.PostMessage("PWF API Tool Error: " + e.ToString(), "PWF API Admin", "C9X3YGW5B");

                return null;
            }
        }

        public JsonResultTimes GetTime(string timeFrom, string timeTo)
        {
            try
            {
                RestSharp.RestResponse response = DoRequest("time?trackedfrom=" + timeFrom + "&trackedto=" + timeTo, "time");
                ProjectsReply = response.Content.ToString();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResultTimes>(ProjectsReply);
            }
            catch(Exception e)
            {
                SlackClient client = new SlackClient("");
                client.PostMessage("PWF API Tool Error: " + e.ToString(), "PWF API Admin", "C9X3YGW5B");

                return null;
            }
        }

        private RestSharp.RestResponse DoRequest(string sRequestedAPI, string type)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(BASEURI + "/" + sRequestedAPI);
            client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(USERNAME, PASSWORD);
            RestRequest request = new RestRequest("", Method.GET);
            request.AddQueryParameter("apikey", APIKEY);
            if (type == "project")
            {
                request.AddParameter("fields", "id,title,manager,startdate,duedate,completedate,category");
            }
            else if (type == "task")
            {
                request.AddParameter("fields", "duedate,startdate,completedate,name,company,category,status,order");
            }
            else if (type == "time")
            {
                request.AddParameter("fields", "company,task,timetracked,project,category,contact,dates,notes,");
            }
            RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();
            return (RestSharp.RestResponse)client.Execute(request);
        }
    }
}
