using AzureFunctions.Extensions.Slack.Bindings;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using System;

namespace AzureFunctions.Extensions.Slack.Config
{
    [Extension("Slack")]
    public partial class SlackExtensionConfig : IExtensionConfigProvider
    {

        void IExtensionConfigProvider.Initialize(ExtensionConfigContext context)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }

            context.AddConverter<string, SlackMessage>(c => new SlackMessage() { Text = c });

            context
                .AddBindingRule<SlackBotAttribute>()
                .BindToCollector(BuildFromAttribute)
                ;

            context
                .AddBindingRule<SlackBotFileUploadAttribute>()
                .BindToCollector(BuildFromAttribute)
                ;

        }

        private IAsyncCollector<SlackMessage> BuildFromAttribute(SlackBotAttribute slackBotAttribute)
        {
            return new PostMessageAsyncCollector(slackBotAttribute);
        }

        private IAsyncCollector<SlackFilesUpload> BuildFromAttribute(SlackBotFileUploadAttribute slackBotFileUploadAttribute)
        {
            return new FileUploadMessageAsyncCollector(slackBotFileUploadAttribute);
        }

    }
}