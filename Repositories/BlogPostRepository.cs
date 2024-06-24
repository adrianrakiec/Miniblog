using Microsoft.EntityFrameworkCore;
using Miniblog.Data;
using Miniblog.Models.Domain;

namespace Miniblog.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly MiniblogDbContext miniblogDbContext;

        public BlogPostRepository(MiniblogDbContext miniblogDbContext)
        {
            this.miniblogDbContext = miniblogDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await miniblogDbContext.BlogPosts.AddAsync(blogPost);
            await miniblogDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingBlogPost = await miniblogDbContext.BlogPosts.FindAsync(id);

            if (existingBlogPost != null)
            {
                miniblogDbContext.BlogPosts.Remove(existingBlogPost);
                await miniblogDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await miniblogDbContext.BlogPosts.Include(nameof(BlogPost.Tags)).ToListAsync();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync(string tagName)
        {
            return await (miniblogDbContext.BlogPosts.Include(nameof(BlogPost.Tags))
                .Where(x => x.Tags.Any(x => x.Name == tagName)))
                .ToListAsync();
        }

        public async Task<BlogPost> GetAsync(Guid id)
        {
            return await miniblogDbContext.BlogPosts.Include(nameof(BlogPost.Tags)).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost> GetAsync(string urlHandle)
        {
            return await miniblogDbContext.BlogPosts.Include(nameof(BlogPost.Tags)).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await miniblogDbContext.BlogPosts.Include(nameof(BlogPost.Tags))
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogPost != null)
            {
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.UrlHandle = blogPost.UrlHandle;
                existingBlogPost.PublishedDate = blogPost.PublishedDate;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.Visible = blogPost.Visible;

                if (blogPost.Tags != null && blogPost.Tags.Any())
                {
                    miniblogDbContext.Tags.RemoveRange(existingBlogPost.Tags);

                    blogPost.Tags.ToList().ForEach(x => x.BlogPostId = existingBlogPost.Id);
                    await miniblogDbContext.Tags.AddRangeAsync(blogPost.Tags);
                }
            }

            await miniblogDbContext.SaveChangesAsync();
            return existingBlogPost;
        }
    }
}
