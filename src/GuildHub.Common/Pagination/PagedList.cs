using Microsoft.EntityFrameworkCore;

namespace GuildHub.Common.Pagination;

public sealed class PagedList<TEntity> where TEntity : Entity
{
    public List<TEntity> EntitiesInPage { get; }
    public int CurrentPageIndex { get; }
    public int EntitiesPerPage { get; }
    public int EntitiesCount { get; }
    public int PagesCount { get; }

    private PagedList(List<TEntity> entitiesInPage, int currentPageIndex, int entitiesPerPage, int entitiesCount, int pagesCount)
    {
        EntitiesInPage = entitiesInPage;
        CurrentPageIndex = currentPageIndex;
        EntitiesPerPage = entitiesPerPage;
        EntitiesCount = entitiesCount;
        PagesCount = pagesCount;
    }

    public static async Task<PagedList<TEntity>> BuildAsync(
        IQueryable<TEntity> entities,
        int? currentPageIndex,
        int? entitiesPerPage,
        CancellationToken cancellationToken)
    {
        int entitiesCount = await entities.CountAsync(cancellationToken);
        int validatedEntitiesPerPage = entitiesPerPage is null || entitiesPerPage > 50 ? 50 : entitiesPerPage.Value;
        validatedEntitiesPerPage = validatedEntitiesPerPage <= 0 ? 1 : validatedEntitiesPerPage;
        int pagesCount = (int)Math.Ceiling(entitiesCount / (double)validatedEntitiesPerPage);
        int validatedCurrentPageIndex = currentPageIndex is null || currentPageIndex <= 1 ? 1 : currentPageIndex.Value;
        validatedCurrentPageIndex = validatedCurrentPageIndex > pagesCount ? pagesCount : validatedCurrentPageIndex;
        var entitiesInPage = new List<TEntity>();
        if (entitiesCount > 0)
        {
            entitiesInPage = await entities
                .Skip((validatedCurrentPageIndex - 1) * validatedEntitiesPerPage)
                .Take(validatedEntitiesPerPage)
                .ToListAsync(cancellationToken);
        }
        return new PagedList<TEntity>(entitiesInPage, validatedCurrentPageIndex, validatedEntitiesPerPage, entitiesCount, pagesCount);
    }
}
