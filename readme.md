[![Build Status](https://dev.azure.com/solita-episerver-public/slack-notifier/_apis/build/status/solita-yusuftiryaki.slack-notifier?branchName=master)](https://dev.azure.com/solita-episerver-public/slack-notifier/_build/latest?definitionId=7&branchName=master)

# Solita Slack Notifier
Easy AF API to send Slack room notifications & log4net appender.

1. Select the channel to notify
2. Select "+ Add an app or custom integration"
4. Search and select "Incoming WebHooks"
5. Click "Add Configuration"
6. Ensure the channel is correct
7. Click "Add Incoming WebHooks integration"
8. "*Webhook URL*" is value for **SlackEndpointUrl**

## Log4net appender
### Simple log4net.config example

```xml
<?xml version="1.0" encoding="utf-8"?>
<log4net>

  <!-- Custom Slack appender -->
  <appender name="SlackAppender" type="Solita.Slack.Notifier.Log4net.SlackAppender">
   <!-- Use this to indicate project and environment -->
    <slackSender>XXX PROD-01</slackSender>
    <!-- Endpoint url for the integration -->
    <slackEndpointUrl>https://hooks.slack.com/services/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</slackEndpointUrl>
  </appender>
  
  <!-- Custom Slack logger -->
  <logger name="SlackLogger" additivity="false">
    <level value="ALL" />
    <appender-ref ref="SlackAppender" />
  </logger>
  
</log4net>
```

### Code example
```csharp
        private static readonly ILog SlackLogger = LogManager.GetLogger("SlackLogger");

        private static void Index()
        {
            SlackLogger.Debug("Meikäläisen aikataulu on sellainen, että ikinä ei olla myöhässä mistään");
            SlackLogger.Info("Ennen oli poliisit sentään poliiseja ja laivat rautaa. Nyt on poliisit peltiä ja laivat mitä lie lasikuitua.");
            SlackLogger.Warn("En suosittele antabusta kenellekään. Sen kanssa ryypätessä tulee ihan kammottava olo.");
            SlackLogger.Error("Kun ihminen nukkuu, sille ei tapahdu mitään. Mutta kun se ei nuku, se voi saada vaikka kalan.");
            SlackLogger.Fatal("Kivitettiinhän Jeesustakin aikanaan, mutta nyt mies on maailman maineessa.");
        }
```
## Slack room sample
![Slack room sample](documentationResources/slackpost.PNG)

## Without Log4net
You can use SlackNotifier directly. See Test cases for details.
