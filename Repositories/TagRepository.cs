using Microsoft.EntityFrameworkCore;
using Miniblog.Data;
using Miniblog.Models.Domain;

namespace Miniblog.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly MiniblogDbContext miniblogDbContext;

        public TagRepository(MiniblogDbContext miniblogDbContext)
        {
            this.miniblogDbContext = miniblogDbContext;
        }


        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            var tags = await miniblogDbContext.Tags.ToListAsync();

            return tags.DistinctBy(x => x.Name.ToLower());
        }
    }
}
