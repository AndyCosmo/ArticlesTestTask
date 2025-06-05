using ArticlesTestTask.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticlesTestTask.Contracts.Responses
{
    public class ArticleResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public long? SectionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
