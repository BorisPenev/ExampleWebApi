using Application.Core;
using Dapper;
using FluentValidation;
using MediatR;
using Persistence;
using System.Data;
using BCrypt.Net;

namespace Application.Users;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(y => y.Password);
        }
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
                    var query = @"INSERT INTO Users (Title, FirstName, LastName, Email, PasswordHash)
                            VALUES (@Title, @FirstName, @LastName, @Email, @PasswordHash)";
                    var parameters = new
                    {
                        Title = request.Title,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
                    };

                    rowsAffected = await db.ExecuteAsync(query, parameters, transaction, 30, CommandType.Text);

                    transaction.Commit();
                }
                
                db.Close();
            };

            if (rowsAffected <= 0)
            {
                return Result<Unit>.Failure("Failed to create the user!");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
