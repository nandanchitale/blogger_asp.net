using Blogger.Models;

namespace Helpers.ViewModels;

public class PostVM
{
    public long? PostId { get; set; } = null;
    public string? Title { get; set; } = null;
    public string? Content { get; set; } = null;
    public string? Author { get; set; } = null;
    public string? PostTimeStamp { get; set; } = null;

    public IEnumerable<PostCommentsVM>? PostComments { get; set; }
}

public class PostCommentsVM
{
    public long? PostCommentId { get; set; } = null;
    public long PostID { get; set; }
    public string? Commenter { get; set; } = null;
    public string? CommentText { get; set; } = null;
    public string? PostCommentTimeStamp { get; set; } = null;
}