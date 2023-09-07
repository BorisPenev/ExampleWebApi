using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Persistence;

public class SqliteDbConnectionFactory 
{
    protected readonly string dbConnectionString;

    public SqliteDbConnectionFactory(string dbConnectionString)
    {
        this.dbConnectionString = dbConnectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new SqliteConnection(dbConnectionString);
    }

    public async Task Init()
    {
        // create database tables if they don't exist
        using var connection = CreateConnection();
        await _initUsers();

        async Task _initUsers()
        {
            var sql = @"
            CREATE TABLE IF NOT EXISTS 
            Users (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                Title TEXT,
                FirstName TEXT,
                LastName TEXT,
                Email TEXT,
                PasswordHash TEXT
            );

            CREATE TABLE IF NOT EXISTS 
            UploadedFiles (
                Id TEXT NOT NULL PRIMARY KEY,
                Name TEXT,
                DateUploaded TEXT
            );

            CREATE TABLE IF NOT EXISTS 
            ApplicationHeaderBlocks (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                FileId TEXT NOT NULL,
                Value TEXT
            );
            CREATE TABLE IF NOT EXISTS 
            BasicHeaderBlocks (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                FileId TEXT NOT NULL,
                Value TEXT
            );
            CREATE TABLE IF NOT EXISTS 
            TextBlocks (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                FileId TEXT NOT NULL,
                Value TEXT
            );
            CREATE TABLE IF NOT EXISTS 
            TrailerBlocks (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                FileId TEXT NOT NULL,
                Value TEXT
            );
            CREATE TABLE IF NOT EXISTS 
            UserHeaderBlocks (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                FileId INTEGER NOT NULL,
                Value TEXT
            );
            ";
            await connection.ExecuteAsync(sql);
        }
    }
}