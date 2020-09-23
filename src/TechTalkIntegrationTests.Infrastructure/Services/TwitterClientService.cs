using System.Threading.Tasks;
using TechTalkIntegrationTests.Domain.Models.Services;
using Tweetinvi;
using Tweetinvi.Models;

namespace TechTalkIntegrationTests.Infrastructure.Services
{
    public class TwitterClientService : ITwitterClientService
    {
        public async Task<bool> PostTweetAsync(string text)
        {
            Auth.SetCredentials(new TwitterCredentials("1OgklADZZ0dmsfL0BlEjSIcLH", "eHgEjBh55vF20xXvU7j6JuCCA5c2qhFtE1DSxXVadPbDfaJC9d", "764558540497362949-yxvNeGPhwsNE02UQETiY3mfHoD01OXt", "TvC1qQ6TSpBMuHYZRjy8mTKiQmlMJoSZcaIBuMfiE29PE"));
            return (await TweetAsync.PublishTweet(text)) != null;
        }
    }
}
