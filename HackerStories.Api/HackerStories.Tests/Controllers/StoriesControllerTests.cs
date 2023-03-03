using HackerStories.Api.Controllers;
using HackerStories.Api.Models;
using HackerStories.Api.Services;
using HackerStories.Api.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HackerStories.Tests.Controllers
{
    public class StoriesControllerTests
    {
        private readonly Mock<IStoriesService> storiesService;

        public StoriesControllerTests()
        {
            storiesService = new Mock<IStoriesService>();
        }

        [Fact]
        public void GetStories_ShouldReturnOkResult()
        {
            //Arrange
            var stories = Data.GetPaginatedStories();
            int pageSize = 30;
            storiesService.Setup(x => x.GetStoriesAsync(stories.PageNumber, pageSize, "")).ReturnsAsync(stories);
            var storiesController = new StoriesController(storiesService.Object);

            //Act
            var result = storiesController.GetStories(stories.PageNumber, pageSize, "");

            //Assert
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as PaginatedList<Story>;
            Assert.NotNull(model);

            Assert.Equal(model.SearchText, stories.SearchText);
            Assert.Equal(model.PageNumber, stories.PageNumber);
            Assert.Equal(model.Items.Count, stories.Items.Count);
            Assert.Equal(1, storiesService.Invocations.Count);
        }
    }
}