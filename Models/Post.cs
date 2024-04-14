using System;
using System.Collections.Generic;

namespace Blogger.Models;

public partial class Post
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public long AuthorId { get; set; }

    public string PostContent { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime StatusChangeDate { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();
}
