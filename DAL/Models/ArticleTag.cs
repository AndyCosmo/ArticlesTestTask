using System.ComponentModel.DataAnnotations.Schema;

namespace ArticlesTestTask.DAL.Models
{    
    /// <summary>
    /// Промежуточная таблица для связей "Статья-Тег"
    /// </summary>
    [Table("articles_tags")]
    public class ArticleTag : BaseModel
    {
        /// <summary>
        /// Id Статьи
        /// </summary>
        [Column("article_id")]
        public long ArticleId { get; set; }

        /// <summary>
        /// Статья
        /// </summary>
        public Article Article { get; set; }

        /// <summary>
        /// Id Тега
        /// </summary>
        [Column("tag_id")]
        public long TagId { get; set; }

        /// <summary>
        /// Тег
        /// </summary>
        public Tag Tag { get; set; }

        /// <summary>
        /// Порядковый номер тега у статьи
        /// </summary>
        [Column("order")]
        public int Order { get; set; }
    }
}
