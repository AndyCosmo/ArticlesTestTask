using ArticlesTestTask.DAL.Models;

namespace ArticlesTestTask.DAL.Repository.Interfaces
{
    public interface ISectionRepository
    {
        Task<int> GetCount(CancellationToken ct = default);
        Task<List<Section>> GetList(int pageNum = 1, int perPage = 10, CancellationToken ct = default);
        Task<Section?> GetByTagIds(List<long> tagIds, CancellationToken ct = default);
        Task<Section> Add(List<Tag> tags, CancellationToken ct = default);
        Task<bool> Exists(long id, CancellationToken ct = default);
        Task DeleteUnusedSections(CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
