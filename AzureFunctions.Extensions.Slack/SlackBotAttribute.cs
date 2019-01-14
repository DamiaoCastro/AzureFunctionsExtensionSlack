using Microsoft.Azure.WebJobs.Description;
using System;

namespace AzureFunctions.Extensions.Slack
{
    [Binding]
    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Parameter)]
    public sealed class SlackBotAttribute : Attribute
    {

        public SlackBotAttribute(string botUserOAuthAccessTokenKey, string channel)
        {
            if (string.IsNullOrWhiteSpace(botUserOAuthAccessTokenKey)) { throw new ArgumentNullException(nameof(botUserOAuthAccessTokenKey)); }
            if (string.IsNullOrWhiteSpace(channel)) { throw new ArgumentNullException(nameof(channel)); }

            BotUserOAuthAccessTokenKey = botUserOAuthAccessTokenKey;
            Channel = channel;

            //var botUserOAuthAccessToken = Environment.GetEnvironmentVariable(botUserOAuthAccessTokenKey, EnvironmentVariableTarget.Process);
            //if (string.IsNullOrWhiteSpace(botUserOAuthAccessToken)) { throw new ArgumentException($"No setting found for key '{BotUserOAuthAccessTokenKey}'"); }

        }

        /// <summary>
        /// Bot User OAuth Access Token
        /// </summary>
        public string BotUserOAuthAccessTokenKey { get; }

        public string Channel { get; }

        public bool AsUser { get; set; } = false;

    }
}