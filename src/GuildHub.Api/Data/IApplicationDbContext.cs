namespace GuildHub.Api.Data;

public interface IApplicationDbContext
{
    DbSet<Post> Posts { get; set; }
    DbSet<PostReply> PostReplies { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
