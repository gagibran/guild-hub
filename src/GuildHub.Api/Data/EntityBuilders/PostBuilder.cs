namespace GuildHub.Api.Data.EntityBuilders;

public sealed class PostBuilder : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> postBuilder)
    {
        postBuilder
            .ToTable("Posts")
            .HasKey(post => post.Id);
        postBuilder
            .Property(item => item.SearchTsVector)
            .HasComputedColumnSql("to_tsvector('english', coalesce(\"Title\", '') || ' ' || coalesce(\"Content\", ''))", true);
        postBuilder
            .HasIndex(post => post.SearchTsVector)
            .HasMethod("GIN");
        postBuilder
            .ComplexProperty(post => post.Title)
            .Property(title => title.TitleName)
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
            .WithOne(postReply => postReply.Post);
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
