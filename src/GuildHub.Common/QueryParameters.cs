namespace GuildHub.Common;

public sealed record QueryParameters(string? Search, int? CurrentPageIndex, int? EntitiesPerPage, string? SortBy);
