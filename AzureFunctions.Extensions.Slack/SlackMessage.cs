using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzureFunctions.Extensions.Slack
{
    public class SlackMessage
    {

        [JsonProperty("as_user")]
        internal bool AsUser { get; set; }
        [JsonProperty("channel")]
        internal string Channel { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("link_names")]
        public bool LinkNames { get; set; }
        [JsonProperty("attachments")]
        public IEnumerable<SlackMessageAttachment> Attachments { get; set; }

        public class SlackMessageAttachment
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("color")]
            public string Color { get; set; }
            [JsonProperty("text")]
            public string Text { get; set; }
        }
    }
}
