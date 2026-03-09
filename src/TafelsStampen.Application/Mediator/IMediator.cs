namespace TafelsStampen.Application.Mediator;

public interface IMediator
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command);
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
}
