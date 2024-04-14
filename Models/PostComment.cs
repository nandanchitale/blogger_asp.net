using System;
using System.Collections.Generic;

namespace Blogger.Models;

public partial class PostComment
{
    public long Id { get; set; }

    public long PostId { get; set; }

    public long UserId { get; set; }

    public string PostComment1 { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime StatusChangeDate { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
