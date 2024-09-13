using GuildHub.Api.PostReplies;

namespace GuildHub.Api.Data;

public interface IApplicationDbContext
{
    DbSet<Post> Posts { get; }
    DbSet<PostReply> PostReplies { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
