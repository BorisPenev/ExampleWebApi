using Application.Core;
using Dapper;
using FluentValidation;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly SqliteDbConnectionFactory sqliteDbConnectionFactory;

        public Handler(SqliteDbConnectionFactory sqliteDbConnectionFactory)
        {
            this.sqliteDbConnectionFactory = sqliteDbConnectionFactory;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var rowsAffected = 0;
            using (IDbConnection db = this.sqliteDbConnectionFactory.CreateConnection())
            {
                db.Open();

                using (IDbTransaction transaction = db.BeginTransaction())
                {
                    var query = @"DELETE FROM Users WHERE Id = @Id";
                    rowsAffected = await db.ExecuteAsync(query, new { Id = request.Id }, transaction, 30, CommandType.Text);

                    transaction.Commit();
                }

                db.Close();
            };

            if (rowsAffected <= 0)
            {
                return Result<Unit>.Failure("Failed to update the user!");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
