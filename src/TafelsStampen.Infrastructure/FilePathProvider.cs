namespace TafelsStampen.Infrastructure;

public class FilePathProvider
{
    public string DataDirectory { get; }

    public FilePathProvider(string dataDirectory)
    {
        DataDirectory = dataDirectory;
        Directory.CreateDirectory(dataDirectory);
    }

    public string PlayersFile => Path.Combine(DataDirectory, "players.json");
    public string SessionsFile => Path.Combine(DataDirectory, "sessions.json");
    public string HallOfFameFile => Path.Combine(DataDirectory, "halloffame.json");
}
