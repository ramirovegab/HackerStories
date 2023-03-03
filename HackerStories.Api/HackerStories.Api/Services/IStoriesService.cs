using HackerStories.Api.Models;
using HackerStories.Api.Wrappers;

namespace HackerStories.Api.Services
{
    public interface IStoriesService
    {
        Task<PaginatedList<Story>> GetStoriesAsync(int pageNumber, int pageSize, string searchPattern);
    }
}
