namespace ArticlesTestTask.Contracts.Requests
{
    public class ArticleCreateUpdateRequest
    {
        /// <summary>
        /// Название статьи
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список тегов
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
