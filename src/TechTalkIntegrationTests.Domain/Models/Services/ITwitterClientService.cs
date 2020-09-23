using System.Threading.Tasks;

namespace TechTalkIntegrationTests.Domain.Models.Services
{
    public interface ITwitterClientService
    {
        Task<bool> PostTweetAsync(string text);
    }
}
