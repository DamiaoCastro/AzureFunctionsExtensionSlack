using AzureFunctions.Extensions.Slack.Config;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(SlackWebJobStartup), "Slack")]
namespace AzureFunctions.Extensions.Slack.Config
{
    class SlackWebJobStartup : IWebJobsStartup
    {
        void IWebJobsStartup.Configure(IWebJobsBuilder builder)
        {
            builder.UseSlack();
        }
    }
}