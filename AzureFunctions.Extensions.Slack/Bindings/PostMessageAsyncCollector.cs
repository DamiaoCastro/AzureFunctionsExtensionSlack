using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunctions.Extensions.Slack.Bindings
{
    internal class PostMessageAsyncCollector : IAsyncCollector<string>
    {

        public PostMessageAsyncCollector(SlackBotAttribute slackBotAttribute)
        {
            this.slackBotAttribute = slackBotAttribute ?? throw new ArgumentNullException(nameof(slackBotAttribute));
        }

        private List<string> messages = new List<string>();
        private readonly SlackBotAttribute slackBotAttribute;

        Task IAsyncCollector<string>.AddAsync(string item, CancellationToken cancellationToken)
        {
            messages.Add(item);

            return Task.CompletedTask;
        }

        async Task IAsyncCollector<string>.FlushAsync(CancellationToken cancellationToken)
        {
            if (messages.All(c => string.IsNullOrWhiteSpace(c))) { return; }

            var botUserOAuthAccessToken = Environment.GetEnvironmentVariable(slackBotAttribute.BotUserOAuthAccessTokenKey, EnvironmentVariableTarget.Process);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", botUserOAuthAccessToken);

                foreach (var message in messages)
                {
                    if (string.IsNullOrWhiteSpace(message)) { continue; }

                    var objMessage = new
                    {
                        channel = slackBotAttribute.Channel,
                        text = message,
                        as_user = slackBotAttribute.AsUser
                    };

                    var messageText = JsonConvert.SerializeObject(objMessage);

                    var content = new StringContent(messageText, Encoding.UTF8, "application/json");

                    await httpClient.PostAsync("https://slack.com/api/chat.postMessage", content);

                }

            }
        }
    }
}