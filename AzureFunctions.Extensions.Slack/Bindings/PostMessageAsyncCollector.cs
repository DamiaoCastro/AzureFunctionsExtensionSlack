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
    internal class PostMessageAsyncCollector : IAsyncCollector<SlackMessage>
    {

        public PostMessageAsyncCollector(SlackBotAttribute slackBotAttribute)
        {
            this.slackBotAttribute = slackBotAttribute ?? throw new ArgumentNullException(nameof(slackBotAttribute));
        }

        private readonly List<SlackMessage> messages = new List<SlackMessage>();
        private readonly SlackBotAttribute slackBotAttribute;

        Task IAsyncCollector<SlackMessage>.AddAsync(SlackMessage item, CancellationToken cancellationToken)
        {
            messages.Add(item);

            return Task.CompletedTask;
        }

        async Task IAsyncCollector<SlackMessage>.FlushAsync(CancellationToken cancellationToken)
        {
            if (!messages.Any()) { return; }
            if (messages.All(c => c == null)) { return; }

            var botUserOAuthAccessToken = Environment.GetEnvironmentVariable(slackBotAttribute.BotUserOAuthAccessTokenKey, EnvironmentVariableTarget.Process);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", botUserOAuthAccessToken);

                foreach (var message in messages.Where(c => c != null))
                {

                    message.Channel = slackBotAttribute.Channel;
                    message.AsUser = slackBotAttribute.AsUser;
                    
                    var messageText = JsonConvert.SerializeObject(message);

                    var content = new StringContent(messageText, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("https://slack.com/api/chat.postMessage", content);
                    response.EnsureSuccessStatusCode();

                }

            }
        }
    }
}