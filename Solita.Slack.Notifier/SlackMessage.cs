using System.Collections.Generic;
using Newtonsoft.Json;

namespace Solita.Slack.Notifier
{
    /// <summary>
    /// Json Message for Slack
    /// </summary>
    public class SlackMessage
    {
        /// <summary>
        /// Username for slack message
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        /// <summary>
        /// Slack message skeletons
        /// </summary>
        [JsonProperty(PropertyName = "attachments")]
        public IList<Attachment> Attachments { get; set; }

        /// <summary>
        /// Slack message (color and text)
        /// </summary>
        public class Attachment
        {
            /// <summary>
            /// Slack message color
            /// </summary>
            [JsonProperty(PropertyName = "color")]
            public string Color { get; set; }

            /// <summary>
            /// Slack message text
            /// </summary>
            [JsonProperty(PropertyName = "text")]
            public string Text { get; set; }
        }
    }
}