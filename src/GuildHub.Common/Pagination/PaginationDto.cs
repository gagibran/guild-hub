namespace GuildHub.Common.Pagination;

public record PaginationDto(int? CurrentPageIndex, int? EntitiesPerPage, int EntitiesCount, int PagesCount);
