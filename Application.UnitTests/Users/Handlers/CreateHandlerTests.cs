using Application.Core;
using Application.Users;
using Dapper;
using Domain;
using MediatR;
using Persistence;
using System.Data;

namespace Application.UnitTests.Users.Handlers;

public class CreateHandlerTests : IAsyncLifetime
{
    private const string testDbConnectionString = "Data Source=Test.db";
    private SqliteDbConnectionFactory sqliteDbConnectionFactory;

    public CreateHandlerTests()
    {
        this.sqliteDbConnectionFactory = new SqliteDbConnectionFactory(testDbConnectionString);
    }

    [Fact]
    public async Task CreateHandler_With_ValidValueseateCommand_Succeed()
    {
        var command = new Create.Command()
        {
            FirstName = "User1 FN",
            LastName = "User1 LN",
            Title = "User1 Title",
            Email = "valid@email.com",
            Password = "123456",
            ConfirmPassword = "123456",
        };
        
        var requestHandler = new Create.Handler(this.sqliteDbConnectionFactory);
        var result = await requestHandler.Handle(command, CancellationToken.None);
        
        Assert.IsType<Result<Unit>>(result);
        Assert.Empty(result.Errors);

        using (IDbConnection db = this.sqliteDbConnectionFactory.CreateConnection())
        {
            var query = "SELECT * FROM Users";
            var users = await db.QueryAsync<User>(query);
            Assert.Single(users);
        };
    }

    public async Task DisposeAsync()
    {
        using (IDbConnection db = this.sqliteDbConnectionFactory.CreateConnection())
        {
            var query = @"
                DELETE FROM Users; 
                DELETE FROM UserHeaderBlocks;
                DELETE FROM TrailerBlocks;
                DELETE FROM TextBlocks  ;
                DELETE FROM BasicHeaderBlocks ;
                DELETE FROM ApplicationHeaderBlocks;
                DELETE FROM UploadedFiles ;";
            var users = await db.QueryAsync<User>(query);
        };
    }

    public async Task InitializeAsync()
    {
        await sqliteDbConnectionFactory.Init();
    }
}
