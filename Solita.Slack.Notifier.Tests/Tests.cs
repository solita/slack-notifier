using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Solita.Slack.Notifier.Tests
{
    [TestClass]
    public class Tests
    {
        private const string Endpoint = "https://hooks.slack.com/services/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";
        private readonly string[] TestMessages = {
            "ASP.NET shutting down due to CodeDirChangeOrDirectoryRename",

            "The configuration file has been changed by another program. (D:\\Websites\\XXX\\Site\\web.config)",

            "Failed to add custom localization provider.",

            "The type initializer for 'EPiServer.Globalization.GlobalizationSettings' threw an exception. ---> System.Configuration.ConfigurationErrorsException: The configuration file has been changed by another program.",

            "Initialize action failed\n" +
            "  at EPiServer.Shell.Modules.ShellZipArchiveVirtualPathProviderModule.<CreateProviders>d__16.MoveNext()\n" +
            "  at System.Linq.Enumerable.<SelectManyIterator>d__16`2.MoveNext()\n" +
            "  at EPiServer.Framework.Initialization.Internal.ModuleNode.Execute(Action a, String key)\n" +
            "  at EPiServer.Framework.Initialization.Internal.ModuleNode.Initialize(InitializationEngine context)\n" +
            "  at EPiServer.Framework.Initialization.InitializationEngine.InitializeModules()"
        };
        // Message interval to prevent flood-cut-off, 
        private const int MessageIntervalMilliseconds = 100; 

        [TestMethod]
        public void TestComponents()
        {
            var factory = new SlackMessageFactory();
            var location = factory.ResolveCallerMethod(0);

            var debug = factory.CreateMessage(SlackMessageLevel.DEBUG, "XXX DEV",     location, TestMessages[0]);
            var info = factory.CreateMessage(SlackMessageLevel.INFO,   "XXX CI",      location, TestMessages[1]);
            var warn = factory.CreateMessage(SlackMessageLevel.WARN,   "XXX TEST-01", location, TestMessages[2]);
            var error = factory.CreateMessage(SlackMessageLevel.ERROR, "XXX PROD-01", location, TestMessages[3]);
            var fatal = factory.CreateMessage(SlackMessageLevel.FATAL, "XXX PROD-02", location, TestMessages[4]);
            
            var debugJson = factory.ToJson(debug);
            var infoJson = factory.ToJson(info);
            var warnJson = factory.ToJson(warn);
            var errorJson = factory.ToJson(error);
            var fatalJson = factory.ToJson(fatal);

            var client = new SlackHttpClient();
            Assert.IsTrue(client.Post(Endpoint, debugJson));
            Wait();
            Assert.IsTrue(client.Post(Endpoint, infoJson));
            Wait();
            Assert.IsTrue(client.Post(Endpoint, warnJson));
            Wait();
            Assert.IsTrue(client.Post(Endpoint, errorJson));
            Wait();
            Assert.IsTrue(client.Post(Endpoint, fatalJson));

        }
        
        [TestMethod]
        public void TestNotifier()
        {
            var notifier = new SlackNotifier("XXX PROD-01", Endpoint, new SlackMessageFactory(), new SlackHttpClient());
            Assert.IsTrue(notifier.Debug(TestMessages[0]));
            Wait();
            Assert.IsTrue(notifier.Info(TestMessages[1]));
            Wait();
            Assert.IsTrue(notifier.Warn(TestMessages[2]));
            Wait();
            Assert.IsTrue(notifier.Error(TestMessages[3]));
            Wait();
            Assert.IsTrue(notifier.Fatal(TestMessages[4]));
        }

        [TestMethod]
        public void TestNotifierEmptyEndpointUrl()
        {
            var notifier = new SlackNotifier("XXX PROD-02", string.Empty, new SlackMessageFactory(), new SlackHttpClient());
            Assert.IsFalse(notifier.Debug(TestMessages[0]));
            Wait();
            Assert.IsFalse(notifier.Info(TestMessages[1]));
            Wait();
            Assert.IsFalse(notifier.Warn(TestMessages[2]));
            Wait();
            Assert.IsFalse(notifier.Error(TestMessages[3]));
            Wait();
            Assert.IsFalse(notifier.Fatal(TestMessages[4]));
        }

        [TestMethod]
        public void TestNotifierNotify()
        {
            const string location = "Test.Episerver.Email.SendOrderConfirmationEmail";
            const string message = "Client does not have permission to submit mail to this server.\n The server response was: 4.7.1 <nonexistence@solita.fi>: Relay access denied";

            var notifier = new SlackNotifier("XXX PROD-01", Endpoint, new SlackMessageFactory(), new SlackHttpClient());
            Assert.IsTrue(notifier.Notify(SlackMessageLevel.WARN, location, message));
        }

        /// <summary>
        /// Slack API will mitigate flooding; wait between messages.
        /// </summary>
        private static void Wait()
        {
            Thread.Sleep(MessageIntervalMilliseconds);
        }
    }
}
