using ArticlesTestTask.DAL.Models;

namespace ArticlesTestTask.DAL.Repository.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag?> GetByLowerName(string name, CancellationToken ct = default);
        Task<Tag> Add(string name, CancellationToken ct = default);
    }
}
