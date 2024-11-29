using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBigBadBlog.Common;

namespace MyBigBadBlog.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IPostRepository _postRepository;

        public readonly MarkdownPipeline MarkdownPipeline;

        public IEnumerable<(PostMetadata Metadata, string Content)> Posts { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IPostRepository postRepository)
        {
            _logger = logger;
            _postRepository = postRepository;

            MarkdownPipeline = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Posts = await _postRepository.GetPostsAsync(10, 1);

            return Page();
        }
    }
}
