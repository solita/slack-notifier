using System;
using System.Configuration;

namespace Solita.Slack.Notifier
{
    /// <summary>
    /// Facade for sending Notification to Slack
    /// </summary>
    public class SlackNotifier : ISlackNotifier
    {
        private readonly string _sender;
        private readonly string _endpointUrl;
        private readonly SlackMessageFactory _messageFactory;
        private readonly SlackHttpClient _httpClient;

        /// <summary>
        /// Creates Default instance with Slack:Sender and Slack:Endpoint information from configuration
        /// </summary>
        /// <returns></returns>
        public static ISlackNotifier CreateDefaultInstance()
        {
            return new SlackNotifier(
                ConfigurationManager.AppSettings["Slack:Sender"],
                ConfigurationManager.AppSettings["Slack:Endpoint"]);
        }

        /// <summary>
        /// Creates an instance of notification Facade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="endpointUrl"></param>
        public SlackNotifier(string sender, string endpointUrl)
            : this(
                sender,
                endpointUrl,
                new SlackMessageFactory(),
                new SlackHttpClient())
        {
            
        }

        /// <summary>
        /// Creates an instance of notification Facade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="endpointUrl"></param>
        /// <param name="messageFactory"></param>
        /// <param name="httpClient"></param>
        public SlackNotifier(string sender,
                             string endpointUrl,
                             SlackMessageFactory messageFactory,
                             SlackHttpClient httpClient)
        {
            _sender = sender;
            _endpointUrl = endpointUrl;
            _messageFactory = messageFactory;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Send debug information to Slack
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Debug(string message)
        {
            var location = _messageFactory.ResolveCallerMethod(1);
            return Notify(SlackMessageLevel.DEBUG, location, message);
        }

        /// <summary>
        /// Send info information to Slack
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Info(string message)
        {
            var location = _messageFactory.ResolveCallerMethod(1);
            return Notify(SlackMessageLevel.INFO, location, message);
        }

        /// <summary>
        /// Send warning information to Slack
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Warn(string message)
        {
            var location = _messageFactory.ResolveCallerMethod(1);
            return Notify(SlackMessageLevel.WARN, location, message);
        }

        /// <summary>
        /// Send error information to Slack
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Error(string message)
        {
            var location = _messageFactory.ResolveCallerMethod(1);
            return Notify(SlackMessageLevel.ERROR, location, message);
        }
        
        /// <summary>
        /// Send fatal information to Slack
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Fatal(string message)
        {
            var location = _messageFactory.ResolveCallerMethod(1);
            return Notify(SlackMessageLevel.FATAL, location, message);
        }

        /// <summary>
        /// Sends information to Slack. You can disable this with Slack:DisableNotifications and make exceptions bubble to client caller with Slack:ThrowExceptions
        /// </summary>
        /// <param name="level"></param>
        /// <param name="location"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Notify(SlackMessageLevel level, string location, string message)
        {
            var shouldDisableNotifications = ConfigurationManager.AppSettings["Slack:DisableNotifications"] == "true";
            if (string.IsNullOrEmpty(_endpointUrl) || shouldDisableNotifications)
            {
                return false;
            }

            try
            {
                var msg = _messageFactory.CreateMessage(level, _sender, location, message);
                var json = _messageFactory.ToJson(msg);
                return _httpClient.Post(_endpointUrl, json);
            }
            catch (Exception)
            {
                var shouldThrowException = ConfigurationManager.AppSettings["Slack:ThrowExceptions"] == "true";
                if (shouldThrowException)
                {
                    throw;
                }
                return false;
            }
        }
    }
}
