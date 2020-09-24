using System.Threading.Tasks;
using TechTalkIntegrationTests.Domain.Models.Services;
using Tweetinvi;

namespace TechTalkIntegrationTests.Infrastructure.Services
{
    public class TwitterClientService : ITwitterClientService
    {
        public async Task<bool> PostTweetAsync(string text)
        {
            return (await TweetAsync.PublishTweet(text)) != null;
        }
    }
}
