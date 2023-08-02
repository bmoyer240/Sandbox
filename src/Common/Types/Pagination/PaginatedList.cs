using Ardalis.GuardClauses;

namespace Application;

public class PaginatedList<T>
{
    public int  EndPage         { get; }
    public int  Limit           { get; }
    public int  Page            { get; }
    public int  PagesToShow     { get; private set; } = 10;
    public int  StartPage       { get; }
    public int  TotalCount      { get; }
    public int  TotalPages      { get; }
    public bool HasNextPage     { get; }
    public bool HasPreviousPage { get; }

    public IReadOnlyCollection<T> Items { get; }

    public PaginatedList(IReadOnlyCollection<T> items, int count, int page, int limit)
    {
        Page = page <= 0 ? 1 : page;
        TotalCount = count;
        Items      = items;
        Limit      = limit;

        if (count > 0)
        {
            EndPage         = Math.Min(TotalPages, StartPage + PagesToShow - 1);
            StartPage       = Math.Max(1, Page - PagesToShow / 2);
            HasNextPage     = Page < TotalPages;
            HasPreviousPage = Page > 1;
            TotalPages      = (int)Math.Ceiling(count / (double)limit);
        }
    }

    public bool HasLimit() =>
        Limit != -1;

    public PaginatedList<T> UpdatePagesToShow(int total)
    {
        if (total < 0)
        {
            throw new ArgumentException($"Total cannot contain a negative value. '{total}'");
        }

        PagesToShow = total;
        return this;
    }

    public static PaginatedList<T> Empty() =>
        new(Array.Empty<T>(), 0, 0, 0);
}
