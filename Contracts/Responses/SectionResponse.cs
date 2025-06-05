namespace ArticlesTestTask.Contracts.Responses
{
    public class SectionResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int ArticlesCount { get; set; }
        public List<string> Tags { get; set; }
    }
}
