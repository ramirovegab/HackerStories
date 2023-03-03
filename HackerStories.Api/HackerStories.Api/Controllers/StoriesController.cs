using HackerStories.Api.Models;
using HackerStories.Api.Services;
using HackerStories.Api.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace HackerStories.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoriesController : ControllerBase
    {

        private readonly IStoriesService _storiesService;

        public StoriesController(IStoriesService storiesService)
        {
            _storiesService = storiesService;
        }

        [HttpGet(Name = "GetStories")]
        [ProducesResponseType(typeof(PaginatedList<Story>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStories(int pageNumber, int pageSize, string? searchText)
        {
            var stories = await _storiesService.GetStoriesAsync(pageNumber, pageSize, searchText ?? "");
            return Ok(stories);
        }
    }
}