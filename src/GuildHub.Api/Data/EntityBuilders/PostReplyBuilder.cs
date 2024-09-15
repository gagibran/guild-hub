namespace GuildHub.Api.Data.EntityBuilders;

public sealed class PostReplyBuilder : IEntityTypeConfiguration<PostReply>
{
    public void Configure(EntityTypeBuilder<PostReply> postReplyBuilder)
    {
        postReplyBuilder
            .ToTable("PostReplies")
            .HasKey(postReply => postReply.Id);
        postReplyBuilder
            .Property(postReply => postReply.Message)
            .HasColumnName("Message")
            .IsRequired();
        postReplyBuilder
            .Property(postReply => postReply.ImagePath)
            .HasColumnName("ImagePath");
        postReplyBuilder
            .HasOne(postReply => postReply.Post)
            .WithMany(post => post.PostReplies);
        postReplyBuilder
            .Property(postReply => postReply.CreatedAtUtc)
            .HasColumnName("CreatedAt")
            .IsRequired();
        postReplyBuilder
            .Property(postReply => postReply.UpdatedAtUtc)
            .HasColumnName("UpdatedAt");
    }
}
