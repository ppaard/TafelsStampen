namespace TafelsStampen.Application.DTOs;

public record PrestatieSamenvattingDto(
    bool IsEersteGame,
    bool IsNieuwPersoonlijkRecord,
    long HuidigeTijdMs,
    long? VorigeBesteMs,
    int HallOfFameRang,
    int AantalDeelnemers,
    IReadOnlyList<SomVerbeteringDto> VerbeterdeSommen);
