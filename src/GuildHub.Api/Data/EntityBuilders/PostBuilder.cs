namespace GuildHub.Api.Data.EntityBuilders;

public class PostBuilder : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> postBuilder)
    {
        postBuilder
            .ToTable("Posts")
            .HasKey(post => post.Id);
        postBuilder
            .Property(post => post.Title)
            .HasColumnName("Title")
            .IsRequired();
        postBuilder
            .Property(post => post.Content)
            .HasColumnName("Content");
        postBuilder
            .Property(post => post.ImagePath)
            .HasColumnName("ImagePath");
        postBuilder
            .HasMany(post => post.PostReplies)
            .WithOne(postReply => postReply.Post)
            .HasForeignKey("PostId");
        postBuilder
            .Property(post => post.CreatedAtUtc)
            .HasColumnName("CreatedAt")
            .IsRequired();
        postBuilder
            .Property(post => post.UpdatedAtUtc)
            .HasColumnName("UpdatedAt");
    }
}
