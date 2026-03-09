namespace TafelsStampen.Application.Mediator;

public interface ICommand<TResult> { }
public interface ICommand : ICommand<Unit> { }
