using Microsoft.EntityFrameworkCore;

namespace GuildHub.Common.Pagination;

public sealed class PagedList<TEntity> where TEntity : Entity
{
    public List<TEntity> Entities { get; }
    public int CurrentPage { get; }
    public int PageSize { get; }
    public int TotalAmountOfEntities { get; }
    public int TotalNumberOfPages { get; }

    private PagedList(List<TEntity> entities, int currentPage, int pageSize, int totalAmountOfEntities, int totalNumberOfPages)
    {
        Entities = entities;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalAmountOfEntities = totalAmountOfEntities;
        TotalNumberOfPages = totalNumberOfPages;
    }

    public static async Task<PagedList<TEntity>> CreatePagedListAsync(
        IQueryable<TEntity> entities,
        int? currentPage,
        int? pageSize,
        CancellationToken cancellationToken)
    {
        var totalAmountOfEntities = await entities.CountAsync(cancellationToken);
        int actualCurrentPage = currentPage is null || currentPage <= 1 ? 1 : currentPage.Value;
        int actualPageSize = pageSize is null || pageSize > 50 ? 50 : pageSize.Value;
        actualPageSize = actualPageSize <= 0 ? 1 : actualPageSize;
        int totalNumberOfPages = (int)Math.Ceiling(totalAmountOfEntities / (double)actualPageSize);
        actualCurrentPage = actualCurrentPage > totalNumberOfPages ? totalNumberOfPages : actualCurrentPage;
        var entitiesInPage = new List<TEntity>();
        if (totalAmountOfEntities > 0)
        {
            entitiesInPage = await entities
                .Skip((actualCurrentPage - 1) * actualPageSize)
                .Take(actualPageSize)
                .ToListAsync(cancellationToken);
        }
        return new PagedList<TEntity>(entitiesInPage, actualCurrentPage, actualPageSize, totalAmountOfEntities, totalNumberOfPages);
    }
}
