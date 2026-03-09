namespace TafelsStampen.Application.Mediator;

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command);
}

public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand { }
