// Test the database connection from the local machine.
// Update userEmail and accessToken after first run, see below for details.
// Execute the program with:
// dotnet run test-db-connection.cs

#:package Npgsql@10.0.1

using System;
using System.Threading.Tasks;
using Npgsql;

// get current az connection user email by executing in a console:
// az account show --query user.name --output tsv

var userEmail = "your-azure-account@outlook.com"; // paste user email here

// get access token by executing in a console:
// az account get-access-token --resource https://ossrdbms-aad.database.windows.net --query accessToken --output tsv

var accessToken = "ey..."; // paste access token here

var databaseHost = "psql-wiok5xjx8aqja.postgres.database.azure.com"; // update the database server as per Azure Portal

string connectionString = $"Host={databaseHost}" +
                          ";Database=neshopdb" +
                          ";Port=5432" +
                          $";Username={userEmail}" +
                          $";Password={accessToken}" +
                          ";Ssl Mode=Require;";

Console.WriteLine("Testing Database Connection\n");

try
{
    await using var connection = new NpgsqlConnection(connectionString);

    Console.WriteLine("Opening connection...");
    await connection.OpenAsync();

    Console.WriteLine("Connected succesfully, connection details:");

    Console.WriteLine($"  Server Version: {connection.PostgreSqlVersion}");
    Console.WriteLine($"  Database:       {connection.Database}");
    Console.WriteLine($"  Host:           {connection.Host}");
    Console.WriteLine($"  Port:           {connection.Port}\n");

    Console.WriteLine("Executing test query...");
    await using var command = new NpgsqlCommand("SELECT NOW() as current_time, version() as pg_version", connection);
    await using var reader = await command.ExecuteReaderAsync();

    if (await reader.ReadAsync())
    {
        Console.WriteLine("Query executed successfully, result:");
        Console.WriteLine($"  Current Time: {reader["current_time"]}");
        Console.WriteLine($"  PostgreSQL Version: {reader["pg_version"]}\n");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Details: {ex.GetType().Name}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
    Console.WriteLine("");
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
