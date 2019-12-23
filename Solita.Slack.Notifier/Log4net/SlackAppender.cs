using log4net.Appender;
using log4net.Core;

namespace Solita.Slack.Notifier.Log4net
{
    /// <summary>
    /// Log4net appender for Slack
    /// </summary>
    public class SlackAppender : AppenderSkeleton
    {
        /// <summary>
        /// Username for slack message
        /// </summary>
        public string SlackSender { get; set; }
        /// <summary>
        /// Slack endpoint
        /// </summary>
        public string SlackEndpointUrl { get; set; }
        
        /// <summary>
        /// Appends message to Slack
        /// </summary>
        /// <param name="eve"></param>
        protected override void Append(LoggingEvent eve)
        {
            var level = GetSlackLevel(eve.Level);
            var location = $"{eve.LocationInformation.ClassName}.{eve.LocationInformation.MethodName}";
            
            var notifier = new SlackNotifier(SlackSender, SlackEndpointUrl, new SlackMessageFactory(), new SlackHttpClient());
            notifier.Notify(level, location, eve.MessageObject.ToString());
        }

        private static SlackMessageLevel GetSlackLevel(Level level)
        {
            if (level <= Level.Debug)
            {
                return SlackMessageLevel.DEBUG;
            }

            if (level <= Level.Info)
            {
                return SlackMessageLevel.INFO;
            }

            if (level <= Level.Warn)
            {
                return SlackMessageLevel.WARN;
            }

            if (level <= Level.Error)
            {
                return SlackMessageLevel.ERROR;
            }
            
            return SlackMessageLevel.FATAL;
        }
    }
}