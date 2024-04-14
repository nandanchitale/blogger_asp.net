using System;
using System.Collections.Generic;

namespace Blogger.Models;

public partial class User
{
    public long Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string Status { get; set; } = null!;

    public DateTime StatusChangeDate { get; set; }

    public virtual ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
