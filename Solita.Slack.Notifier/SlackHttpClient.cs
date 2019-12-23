using System;
using System.Net.Http;
using System.Text;

namespace Solita.Slack.Notifier
{
    /// <summary>
    /// Http client for sending Slack messages
    /// </summary>
    public class SlackHttpClient
    {
        /// <summary>
        /// Post messages to Slack
        /// </summary>
        /// <param name="endpointUrl"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool Post(string endpointUrl, string json)
        {
            using (var client = new HttpClient())
            {
                // 3s max timeout should do
                client.Timeout = TimeSpan.FromSeconds(3);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpointUrl, content).Result;

                return result.IsSuccessStatusCode;
            }
        }
    }
}