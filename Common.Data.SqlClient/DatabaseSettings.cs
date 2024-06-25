using System.ComponentModel.DataAnnotations;

namespace Common.Data.SqlClient;

public class DatabaseSettings
{
    public static readonly DatabaseSettings EmptyDatabaseSettings = new();

    [Required] public string ConnectionStringTemplate { get; init; } = string.Empty;

    [Required] public string File { get; init; } = string.Empty;
}