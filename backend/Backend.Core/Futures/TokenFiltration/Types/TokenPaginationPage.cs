namespace Backend.Core.Futures.TokenFiltration.Types;

public class TokenPaginationPage<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalSize { get; set; }
    public IEnumerable<T> Items { get; set; } = default!;
}