using Miniblog.Models.Domain;

namespace Miniblog.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
    }
}
