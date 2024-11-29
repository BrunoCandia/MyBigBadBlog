using Microsoft.EntityFrameworkCore;
using MyBigBadBlog.Common;

namespace MyBigBadBlog.Data.Postgres
{
    public class PgRepository : IPostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PgRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(PostMetadata, string)> GetPostAsync(string slug)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Slug == slug);

            if (post == null)
            {
                return (null, null);
            }

            return ((PostMetadata)post, post.Content);
        }

        public async Task<(PostMetadata, string)> GetPostByIdAsync(int id)
        {
            var post = await _dbContext.Posts.FindAsync(id);

            if (post == null)
            {
                return (null, null);
            }

            return ((PostMetadata)post, post.Content);
        }

        public async Task<IEnumerable<(PostMetadata, string)>> GetPostsAsync(int count, int page)
        {
            var posts = await _dbContext.Posts
                .OrderByDescending(p => p.Date)
                .Skip((page - 1) * count)
                .Take(count)
                .ToListAsync();

            return posts.Select(p => ((PostMetadata)p, p.Content));
        }

        public async Task AddPostAsync(PostMetadata post, string content)
        {
            var pgPost = (PgPost)post;
            pgPost.Date = new DateTime(pgPost.Date.Year, pgPost.Date.Month, pgPost.Date.Day, 0, 0, 0, DateTimeKind.Utc);
            pgPost.Content = content;

            await _dbContext.Posts.AddAsync(pgPost);
            await _dbContext.SaveChangesAsync();
        }
    }
}
