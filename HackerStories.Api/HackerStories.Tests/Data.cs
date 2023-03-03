using HackerStories.Api.Models;
using HackerStories.Api.Wrappers;

namespace HackerStories.Tests
{
    public static class Data
    {
        public static PaginatedList<Story> GetPaginatedStories()
        {
            return new PaginatedList<Story>(new List<Story>
            {
                new Story()
                {
                    Id = 1,
                    Title = "Title1",
                    Type = "story",
                    Url = "url/1"
                },
                new Story()
                {
                    Id = 2,
                    Title = "Title2",
                    Type = "story",
                    Url = "url/2"
                },
                new Story()
                {
                    Id = 3,
                    Title = "Title3",
                    Type = "story",
                    Url = "url/3"
                }
            }, 1, 30, 3, "");
        }
    }
}
