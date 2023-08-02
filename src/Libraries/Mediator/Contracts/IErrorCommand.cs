namespace Mediator;

public interface IErrorCommand : IErrorCommand<Success>
{
}

public interface IErrorCommand<TResponse> : ICommand<ErrorOr<TResponse>>, IErrorCommandBase
{
}

public interface IErrorCommandBase {}

public interface IErrorCommandHandler<in TCommand> : IErrorCommandHandler<TCommand, Success>
where TCommand : IErrorCommand
{
}

public interface IErrorCommandHandler<in TCommand, TResponse> : ICommandHandler<TCommand, ErrorOr<TResponse>>
where TCommand : IErrorCommand<TResponse>
{
}