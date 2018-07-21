using System;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;

namespace PWF_Report_Updater
{
    public class SlackClient
    {
        private readonly Uri _uri;
        private readonly Encoding _encoding = new UTF8Encoding();

        public SlackClient(string sContactID)
        {
            _uri = new Uri(GetWebhook(sContactID));
        }

        //Post a message using simple strings
        public void PostMessage(string text, string username = null, string channel = null)
        {
            SlackPayload payload = new SlackPayload()
            {
                Channel = channel,
                Username = username,
                Text = text
            };

            SendMessage(payload);
        }

        private void SendMessage(SlackPayload payload)
        {
            string payloadJson = JsonConvert.SerializeObject(payload);

            using (WebClient client = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["payload"] = payloadJson;

                var response = client.UploadValues(_uri, "POST", data);

                //The response text is usually "ok"
                string responseText = _encoding.GetString(response);
            }
        }

        public string GetWebhook(string sContactID)
        {
            string userWebhook = "webhook link";
            switch (sContactID)
            {
                // Removed for security
                default:   
                    userWebhook += "userWebhook";
                    break;
            }
            return userWebhook;
        }
    }

    class SlackPayload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
