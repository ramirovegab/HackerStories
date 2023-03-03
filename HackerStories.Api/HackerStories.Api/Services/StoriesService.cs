using HackerStories.Api.Models;
using HackerStories.Api.Wrappers;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace HackerStories.Api.Services
{
    public class StoriesService : IStoriesService
    {
        private const string StoryIdsCacheKey = "StoryIds";
        private const string UpdatedStoryIdsCacheKey = "UpdatedStoryIds";

        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public StoriesService(IMemoryCache memoryCache, HttpClient httpClient, IConfiguration config)
        {
            _memoryCache = memoryCache;
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<PaginatedList<Story>> GetStoriesAsync(int pageNumber, int pageSize, string searchText)
        {
            List<int> storyIds = await FromCache(StoryIdsCacheKey,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(_config.GetValue<int>("AppSettings:NewStoriesListExpirationHours"))),
                () => GetResponseAsync<List<int>>(_config.GetValue<string>("AppSettings:NewItemsUrl")));

            ListResponse updatedStoryIds = await FromCache(UpdatedStoryIdsCacheKey,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(_config.GetValue<int>("AppSettings:UpdatedStoriesListExpirationHours"))),
                () => GetResponseAsync<ListResponse>(_config.GetValue<string>("AppSettings:UpdatedItemsUrl")));

            IEnumerable<Story> stories = await GetStoriesAsync(storyIds, updatedStoryIds.Items);

            List<Story> validStories = new List<Story>();
            foreach (var story in stories) 
            {
                if (story.Type == "story" && !string.IsNullOrEmpty(story.Url) && 
                    (!string.IsNullOrEmpty(searchText) ? story.Title.Contains(searchText) : true))
                {
                    validStories.Add(story);
                }
            }
            return PaginatedList<Story>.CreateAsync(validStories, pageNumber, pageSize, searchText);
        }

        private async Task<IEnumerable<Story>> GetStoriesAsync(IEnumerable<int> storyIds, IEnumerable<int> updatedStoryIds)
        {
            var storyUrl = $"{_config.GetValue<string>("AppSettings:StoryUrl")}";
            List<Story> stories = new List<Story>();
            await Parallel.ForEachAsync(storyIds, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (storyId, token) =>
            {
                Story story = updatedStoryIds.Contains(storyId) ? await GetResponseAsync<Story>($"{storyUrl}{storyId}.json") :
                    await FromCache(storyId.ToString(), new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(_config.GetValue<int>("AppSettings:NewStoriesListExpirationHours"))),
                    () => GetResponseAsync<Story>($"{storyUrl}{storyId}.json"));
                stories.Add(story);
            });
            return stories;
            /**************
            foreach (var storyId in storyIds)
            {
                var story = updatedStoryIds.Contains(storyId) ? await GetResponseAsync<Story>($"{storyUrl}{storyId}.json") :
                    await FromCache(storyId.ToString(), new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(_config.GetValue<int>("AppSettings:NewStoriesListExpirationHours"))),
                    () => GetResponseAsync<Story>($"{storyUrl}{storyId}.json"));
                yield return story;
            }*/

        }

        private async Task<T> GetResponseAsync<T>(string relativeUri)
        {
            T result;
            using (HttpResponseMessage response = await _httpClient.GetAsync(relativeUri))
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            return default;
        }

        private async Task<T> FromCache<T>(string key, MemoryCacheEntryOptions cacheOptions, Func<Task<T>> getValueAsync)
        {
            if (!_memoryCache.TryGetValue(key, out T result))
            {
                result = await getValueAsync();
                _memoryCache.Set(key, result, cacheOptions);
            }
            return result;
        }
    }
}
