using Application.Core;
using Dapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using System.Data;
using System.Reflection;

namespace Application.Users;

public class Update
{
    public class Command : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
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
                    var searchQuery = "SELECT * FROM Users WHERE Id = @id";
                    var user = await db.QuerySingleOrDefaultAsync<User>(searchQuery, new { Id = request.Id });

                    if (user == null)
                    {
                        return null;
                    }

                    var emailChanged = !string.IsNullOrEmpty(request.Email) && user.Email != request.Email;
                    var searchByEmail = "SELECT * FROM Users WHERE Email = @email";
                    var duplicateUser = emailChanged && (await db.QuerySingleOrDefaultAsync<User>(searchByEmail, new { Email = request.Email }) != null);
                    if (emailChanged && duplicateUser)
                    {
                        return Result<Unit>.Failure($"User with the email {request.Email} already exists");
                    }

                    if (!string.IsNullOrEmpty(request.Password))
                    {
                        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    }

                    user.Title = request.Title;
                    user.Email = request.Email;
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;

                    var query = @"
                    UPDATE Users 
                    SET Title = @Title,
                        FirstName = @FirstName,
                        LastName = @LastName, 
                        Email = @Email, 
                        Role = @Role, 
                        PasswordHash = @PasswordHash
                    WHERE Id = @Id";
                    rowsAffected = await db.ExecuteAsync(query, user, transaction, 30, CommandType.Text);

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
