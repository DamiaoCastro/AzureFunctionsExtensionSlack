using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunctions.Extensions.Slack.Bindings
{
    internal class FileUploadMessageAsyncCollector : IAsyncCollector<SlackFilesUpload>
    {

        public FileUploadMessageAsyncCollector(SlackBotFileUploadAttribute slackBotFileUploadAttribute)
        {
            this.slackBotFileUploadAttribute = slackBotFileUploadAttribute ?? throw new ArgumentNullException(nameof(slackBotFileUploadAttribute));
        }

        private readonly List<SlackFilesUpload> messages = new List<SlackFilesUpload>();
        private readonly SlackBotFileUploadAttribute slackBotFileUploadAttribute;

        Task IAsyncCollector<SlackFilesUpload>.AddAsync(SlackFilesUpload item, CancellationToken cancellationToken)
        {
            messages.Add(item);

            return Task.CompletedTask;
        }

        async Task IAsyncCollector<SlackFilesUpload>.FlushAsync(CancellationToken cancellationToken)
        {
            if (messages.All(c => c == null)) { return; }

            var botUserOAuthAccessToken = Environment.GetEnvironmentVariable(slackBotFileUploadAttribute.BotUserOAuthAccessTokenKey, EnvironmentVariableTarget.Process);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", botUserOAuthAccessToken);

                foreach (var message in messages)
                {
                    if (message is null) { continue; }

                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(slackBotFileUploadAttribute.Channel), "channels");

                    if (!string.IsNullOrWhiteSpace(message.filename)) { content.Add(new StringContent(message.filename), "filename"); }
                    if (!string.IsNullOrWhiteSpace(message.filetype)) { content.Add(new StringContent(message.filetype), "filetype"); }
                    if (!string.IsNullOrWhiteSpace(message.initial_comment)) { content.Add(new StringContent(message.initial_comment), "initial_comment"); }
                    if (!string.IsNullOrWhiteSpace(message.title)) { content.Add(new StringContent(message.title), "title"); }
                    if (!string.IsNullOrWhiteSpace(message.content))
                    {
                        content.Add(new StreamContent(new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(message.content))), "file", message.filename);
                    }

                    var r = await httpClient.PostAsync("https://slack.com/api/files.upload", content, cancellationToken).ConfigureAwait(false);
                    r.EnsureSuccessStatusCode();

                }

            }
        }

    }
}
