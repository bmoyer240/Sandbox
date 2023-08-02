namespace Mediator;

public interface IPagedQuery<TResponse> : IQuery<PaginatedList<TResponse>>
{
}

public interface IPagedQueryHandler<in TQuery, TResponse> : IQueryHandler<TQuery, PaginatedList<TResponse>>
where TQuery : IPagedQuery<TResponse>
{
}