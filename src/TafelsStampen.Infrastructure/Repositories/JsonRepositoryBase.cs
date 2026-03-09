namespace TafelsStampen.Infrastructure.Repositories;
using System.Text.Json;

public abstract class JsonRepositoryBase<T>
{
    private readonly string _filePath;
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    protected JsonRepositoryBase(string filePath)
    {
        _filePath = filePath;
    }

    protected async Task<List<T>> ReadAllAsync()
    {
        if (!File.Exists(_filePath))
            return new List<T>();

        var json = await File.ReadAllTextAsync(_filePath);
        if (string.IsNullOrWhiteSpace(json))
            return new List<T>();

        return JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
    }

    protected async Task WriteAllAsync(List<T> items)
    {
        var json = JsonSerializer.Serialize(items, Options);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
