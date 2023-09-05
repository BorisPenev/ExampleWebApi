using Application.Core;
using Dapper;
using Domain;
using MediatR;
using Persistence;
using System.Data;

namespace Application.Users;

public class GetById
{
    public class Query : IRequest<Result<User>> 
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<User>>
    {
        private readonly SqliteDbConnectionFactory sqliteDbConnectionFactory;

        public Handler(SqliteDbConnectionFactory sqliteDbConnectionFactory)
        {
            this.sqliteDbConnectionFactory = sqliteDbConnectionFactory;
        }

        public async Task<Result<User>> Handle(Query request, CancellationToken cancellationToken)
        {
            using (IDbConnection db = this.sqliteDbConnectionFactory.CreateConnection())
            {
                var query = "SELECT * FROM Users WHERE Id = @id";
                var foundUser = await db.QuerySingleOrDefaultAsync<User>(query, new { Id = request.Id });

                if (foundUser == null)
                {
                    return null;
                }

                return Result<User>.Success(foundUser);
            };

        }
    }
}