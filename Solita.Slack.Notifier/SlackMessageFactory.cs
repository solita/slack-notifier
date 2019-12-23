using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Solita.Slack.Notifier
{
    /// <summary>
    /// Generates json for Slack
    /// </summary>
    public class SlackMessageFactory
    {
        /// <summary>
        /// Create a Slack message
        /// </summary>
        /// <param name="level"></param>
        /// <param name="sender"></param>
        /// <param name="location"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public SlackMessage CreateMessage(SlackMessageLevel level, string sender, string location, string message)
        {
            return new SlackMessage
            {
                Username = sender,
                Attachments = new [] { new SlackMessage.Attachment
                {
                    Text = $"{level} {location}:\n {message}",
                    Color = MessageLevelColors[level],
                } }
            };
        }

        /// <summary>
        /// Create a Slack message json
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string ToJson(SlackMessage message)
        {
            return JsonConvert.SerializeObject(message);
        }

        /// <summary>
        /// Finds stacktrace for method
        /// </summary>
        public string ResolveCallerMethod(int depth)
        {
            // frame 0 is the current method; increment by one
            var frameIndex = depth + 1;

            var stack = new StackTrace();
            if (stack.FrameCount < frameIndex)
            {
                return string.Empty;
            }

            var method = stack.GetFrame(frameIndex).GetMethod();
            return $"{method.DeclaringType?.FullName}.{method.Name}";
        }

        private readonly IDictionary<SlackMessageLevel, string> MessageLevelColors = new Dictionary<SlackMessageLevel, string>
        {
            { SlackMessageLevel.DEBUG, "good" },
            { SlackMessageLevel.INFO,  "good" },
            { SlackMessageLevel.WARN,  "warning" },
            { SlackMessageLevel.ERROR, "danger" },
            { SlackMessageLevel.FATAL, "danger" },
        };
    }
}