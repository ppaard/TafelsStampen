namespace TafelsStampen.Application;
using Microsoft.Extensions.DependencyInjection;
using TafelsStampen.Application.Commands.FinishGame;
using TafelsStampen.Application.Commands.RegisterPlayer;
using TafelsStampen.Application.Commands.StartGame;
using TafelsStampen.Application.Commands.SubmitAnswer;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Application.Queries.GetGameResult;
using TafelsStampen.Application.Queries.GetHallOfFameByTable;
using TafelsStampen.Application.Queries.GetHallOfFameOverall;
using TafelsStampen.Application.Queries.GetPlayers;
using TafelsStampen.Application.Queries.GetPrestatieSamenvatting;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator.Mediator>();

        // Commands
        services.AddScoped<ICommandHandler<RegisterPlayerCommand, Guid>, RegisterPlayerCommandHandler>();
        services.AddScoped<ICommandHandler<StartGameCommand, Guid>, StartGameCommandHandler>();
        services.AddScoped<ICommandHandler<SubmitAnswerCommand, bool>, SubmitAnswerCommandHandler>();
        services.AddScoped<ICommandHandler<FinishGameCommand, Unit>, FinishGameCommandHandler>();

        // Queries
        services.AddScoped<IQueryHandler<GetPlayersQuery, IReadOnlyList<PlayerDto>>, GetPlayersQueryHandler>();
        services.AddScoped<IQueryHandler<GetHallOfFameByTableQuery, IReadOnlyList<HallOfFameEntryDto>>, GetHallOfFameByTableQueryHandler>();
        services.AddScoped<IQueryHandler<GetHallOfFameOverallQuery, IReadOnlyList<HallOfFameEntryDto>>, GetHallOfFameOverallQueryHandler>();
        services.AddScoped<IQueryHandler<GetGameResultQuery, GameResultDto>, GetGameResultQueryHandler>();
        services.AddScoped<IQueryHandler<GetPrestatieSamenvattingQuery, PrestatieSamenvattingDto>, GetPrestatieSamenvattingQueryHandler>();

        return services;
    }
}
