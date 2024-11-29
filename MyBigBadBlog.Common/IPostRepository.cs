namespace MyBigBadBlog.Common
{
    public interface IPostRepository
    {
        Task<IEnumerable<(PostMetadata, string)>> GetPostsAsync(int count, int page);

        Task<(PostMetadata, string)> GetPostAsync(string slug);

        Task<(PostMetadata, string)> GetPostByIdAsync(int id);

        Task AddPostAsync(PostMetadata post, string content);
    }

    public record PostMetadata(int Id, string Title, string Author, DateTime Date)
    {
        public string Slug => Uri.EscapeDataString(Title.ToLower());

        ////public string Slug
        ////{
        ////    get
        ////    {
        ////        return Uri.EscapeDataString(Title.ToLower());
        ////    }
        ////}
    }
}
