using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Collections.Specialized.BitVector32;

namespace ArticlesTestTask.DAL.Models
{
    /// <summary>
    /// Тег
    /// </summary>
    [Table("tags")]
    public class Tag : BaseModel
    {
        /// <summary>
        /// Название
        /// </summary>
        [Column("name")]
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Название в нижнем регистре для индексации
        /// </summary>
        [Column("name_lower")]
        [Required]
        [MaxLength(256)]
        public string NameLower { get; private set; }

        /// <summary>
        /// Список связей Тег-Статья
        /// </summary>
        [MaxLength(256)]
        public List<ArticleTag> Articles { get; set; } = new List<ArticleTag>();

        /// <summary>
        /// Список разделов у тега
        /// </summary>
        public List<Section> Sections { get; set; } = new List<Section>();

        public void SetName(string name)
        {
            Name = name;
            NameLower = name.ToLowerInvariant();
        }
    }
}
