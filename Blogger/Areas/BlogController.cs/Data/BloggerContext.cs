using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Blogger.Models;

namespace Blogger.Data
{
    public class BloggerContext : DbContext
    {
        public BloggerContext (DbContextOptions<BloggerContext> options)
            : base(options)
        {
        }

        public DbSet<Blogger.Models.Post> Post { get; set; } = default!;
    }
}
