using MyBigBadBlog.Common;
using System.ComponentModel.DataAnnotations;

namespace MyBigBadBlog.Data.Postgres
{
    public class PgPost
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public required string Title { get; set; }

        [Required, MaxLength(100)]
        public required string Author { get; set; }

        [Required]
        public required DateTime Date { get; set; }

        [Required, MaxLength(150)]
        public required string Slug { get; set; }

        public string Content { get; set; }

        //Convert to PostMetadata from PgPost
        public static explicit operator PostMetadata(PgPost post)
        {
            return new PostMetadata(post.Id, post.Title, post.Author, post.Date);
        }

        //Convert to PgPost from PostMetadata
        public static explicit operator PgPost(PostMetadata post)
        {
            return new PgPost
            {
                Id = post.Id,
                Title = post.Title,
                Author = post.Author,
                Date = post.Date,
                Slug = post.Slug
            };
        }

        ////public static explicit operator PgPost((PostMetadata, string) post)
        ////{
        ////    return new PgPost
        ////    {
        ////        Title = post.Item1.Title,
        ////        Author = post.Item1.Author,
        ////        Date = post.Item1.Date,
        ////        Slug = post.Item1.Slug,
        ////        Content = post.Item2
        ////    };
        ////}
    }
}
