using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Blogger.EFCore;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostComment> PostComments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("posts_pk");

            entity.ToTable("posts", "blogger");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('blogger_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.PostContent).HasColumnName("post_content");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.StatusChangeDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("status_change_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.Posts)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("post_auther_fk");
        });

        modelBuilder.Entity<PostComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("post_comments_pk");

            entity.ToTable("post_comments", "blogger");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('blogger_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.PostComment1).HasColumnName("post_comment");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.StatusChangeDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("status_change_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany(p => p.PostComments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("post_fk");

            entity.HasOne(d => d.User).WithMany(p => p.PostComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comment_user_fk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pk");

            entity.ToTable("users", "blogger");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('blogger_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(150)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(150)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .HasColumnName("password");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.StatusChangeDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("status_change_date");
            entity.Property(e => e.Username)
                .HasMaxLength(500)
                .HasColumnName("username");
        });
        modelBuilder.HasSequence("blogger_seq", "blogger");
        modelBuilder.HasSequence("blogger_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
