using AzureFunctions.Extensions.Slack.Config;
using Microsoft.Azure.WebJobs;
using System;

namespace AzureFunctions.Extensions.Slack.Config
{
    public static class JobHostConfigurationExtension
    {
        /// <summary>
        /// Enables use of the Slack extensions for webjobs
        /// </summary>
        /// <param name="config">The <see cref="JobHostConfiguration"/> to configure.</param>
        public static void UseSlack(this IWebJobsBuilder config)
        {

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            // Register our extension configuration provider
            config.AddExtension<SlackExtensionConfig>();

        }

    }
}