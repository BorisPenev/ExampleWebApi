using Application.Core;
using Dapper;
using Domain;
using MediatR;
using Persistence;
using System.Data;

namespace Application.Users;
public class GetAll
{
    public class Query : IRequest<Result<IEnumerable<User>>> { }

    public class Handler : IRequestHandler<Query, Result<IEnumerable<User>>>
    {
        private readonly SqliteDbConnectionFactory sqliteDbConnectionFactory;

        public Handler(SqliteDbConnectionFactory sqliteDbConnectionFactory)
        {
            this.sqliteDbConnectionFactory = sqliteDbConnectionFactory;
        }

        public async Task<Result<IEnumerable<User>>> Handle(Query request, CancellationToken cancellationToken)
        {
            using (IDbConnection db = this.sqliteDbConnectionFactory.CreateConnection())
            {
                var query = "SELECT * FROM Users";
                var allUsers = await db.QueryAsync<User>(query);               

                return Result<IEnumerable<User>>.Success(allUsers);
            };
        }
    }
}