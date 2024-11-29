using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OutputCaching;
using MyBigBadBlog.Common;

namespace MyBigBadBlog.Web.Pages
{
    [OutputCache(PolicyName = "Post")]
    public class PostModel : PageModel
    {
        private readonly IPostRepository _postRepository;
        public readonly MarkdownPipeline MarkdownPipeline;

        public (PostMetadata Metadata, string Content) Post { get; set; }

        public PostModel(IPostRepository postRepository)
        {
            _postRepository = postRepository;

            MarkdownPipeline = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            Post = await _postRepository.GetPostByIdAsync(id);

            if (Post == default)
            {
                return NotFound();
            }

            return Page();
        }

        ////public async Task<IActionResult> OnGetAsync(string slug)
        ////{

        ////    Post = await _postRepository.GetPostAsync(slug);

        ////    if (Post == default)
        ////    {
        ////        return NotFound();
        ////    }

        ////    return Page();
        ////}
    }
}
