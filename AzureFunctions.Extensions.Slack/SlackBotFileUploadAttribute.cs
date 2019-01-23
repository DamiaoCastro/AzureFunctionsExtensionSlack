using Microsoft.Azure.WebJobs.Description;
using System;

namespace AzureFunctions.Extensions.Slack
{
    [Binding]
    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Parameter)]
    public sealed class SlackBotFileUploadAttribute : SlackBotAttribute
    {

        public SlackBotFileUploadAttribute(string botUserOAuthAccessTokenKey, string channel) : base(botUserOAuthAccessTokenKey, channel)
        {
        }
        
    }
}