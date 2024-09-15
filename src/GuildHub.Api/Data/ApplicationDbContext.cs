using GuildHub.Api.Data.EntityBuilders;

namespace GuildHub.Api.Data;

public sealed class ApplicationDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions), IApplicationDbContext
{
    public DbSet<Post> Posts { get; set; } = default!;
    public DbSet<PostReply> PostReplies { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PostBuilder());
        modelBuilder.ApplyConfiguration(new PostReplyBuilder());
    }
}
