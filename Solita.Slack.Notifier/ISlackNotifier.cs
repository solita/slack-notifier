namespace Solita.Slack.Notifier
{
    /// <summary>
    /// Interface for sending information to Slack
    /// </summary>
    public interface ISlackNotifier
    {
        /// <summary>
        /// Send Debug information to Slack
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Debug(string message);
        /// <summary>
        /// Send Info information to Slack
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Info(string message);
        /// <summary>
        /// Send Warn information to Slack
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Warn(string message);
        /// <summary>
        /// Send Error information to Slack
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Error(string message);
        /// <summary>
        /// Send Fatal information to Slack
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Fatal(string message);
        /// <summary>
        /// Send information to Slack
        /// </summary>
        /// <param name="level"></param>
        /// <param name="location"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Notify(SlackMessageLevel level, string location, string message);
    }
}