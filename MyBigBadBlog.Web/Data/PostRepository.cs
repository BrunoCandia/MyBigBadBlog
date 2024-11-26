
namespace MyBigBadBlog.Web.Data
{
    public class PostRepository : IPostRepository
    {
        public Task<(PostMetadata, string)> GetPostAsync(string slug)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<(PostMetadata, string)>> GetPostsAsync(int count, int page)
        {
            throw new NotImplementedException();
        }
    }
}
