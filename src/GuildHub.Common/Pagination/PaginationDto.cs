namespace GuildHub.Common.Pagination;

public record PaginationDto(int? CurrentPage, int? PageSize, int TotalAmountOfItems, int TotalNumberOfPages);
