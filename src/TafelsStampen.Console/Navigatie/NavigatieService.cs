namespace TafelsStampen.Console.Navigatie;

public class NavigatieService
{
    public async Task NaarAsync(IScherm scherm) =>
        await scherm.ToonAsync();
}
