namespace ArticlesTestTask.Contracts.Responses
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }

        public int? Count { get; set; }
    }
}
