using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticlesTestTask.DAL.Models
{
    /// <summary>
    /// Раздел
    /// </summary>
    [Table("sections")]
    public class Section : BaseModel
    {
        /// <summary>
        /// Название
        /// </summary>
        [Column("name")]
        [MaxLength(1024)]
        [ConcurrencyCheck]
        public string Name { get; set; }

        /// <summary>
        /// Список тегов
        /// </summary>
        [MaxLength(256)]
        public List<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// Список статей
        /// </summary>
        public List<Article> Articles { get; set; } = new List<Article>();

        /// <summary>
        /// Счетчик статей в разделе
        /// </summary>
        [Column("articles_count")]
        [ConcurrencyCheck]
        public int ArticlesCount { get; set; }
    }
}
