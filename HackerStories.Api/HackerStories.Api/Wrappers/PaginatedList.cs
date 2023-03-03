namespace HackerStories.Api.Wrappers
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public int TotalCount { get; private set; }
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
        public string SearchText { get; set; }

        public PaginatedList(List<T> items, int pageNumber, int pageSize, int totalCount, string searchText)
        {
            Items = new List<T>();
            PageNumber = pageNumber;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
            SearchText = searchText;
            Items.AddRange(items);
        }

        public static PaginatedList<T> CreateAsync(List<T> source, int pageNumber, int pageSize, string searchText)
        {
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, pageNumber, pageSize, source.Count, searchText);
        }
    }
}
