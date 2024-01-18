namespace UaRestGateway.Console
{
    using Microsoft.Data.Sqlite;

    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "..\\..\\..\\UaRestGateway.sqllite" };
            using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
    
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"
                CREATE TABLE IF NOT EXISTS Accounts (
                    Id INTEGER PRIMARY KEY,
                    WpId INTEGER,
                    Name TEXT,
                    Email TEXT,
                    CompanyName TEXT,
                    MembershipType INTEGER,
                    LastLoginTime TEXT
                );
                CREATE TABLE IF NOT EXISTS Servers (
                    Id INTEGER PRIMARY KEY,
                    EndpointUrl TEXT,
                    SecurityMode INTEGER,
                    SecurityProfileUri TEXT,
                    UserName TEXT,
                    Password TEXT
                );
                ";
            command.ExecuteNonQuery();

        }
    }
}
