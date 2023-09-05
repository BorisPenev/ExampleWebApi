using Application.Core;
using Application.Core.Swift;
using Dapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SwiftMessages
{
    public class UploadFile
    {
        public class Command : IRequest<Result<Unit>>
        {
            public IFormFile File { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.File).SetValidator(new SwiftMessageFileValidator());
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
                var fileId = Guid.NewGuid().ToString();                
                var parser = new MTParser();
                var swiftMessage = new Dictionary<string, string>();

                using (var reader = new StreamReader(request.File.OpenReadStream()))
                {
                    var contentString = await reader.ReadToEndAsync();
                    swiftMessage = parser.SeperateSWIFTFile(contentString);
                }

                using (IDbConnection db = this.sqliteDbConnectionFactory.CreateConnection())
                {
                    db.Open();
                    
                    using (IDbTransaction transaction = db.BeginTransaction()) 
                    {
                        await InsertUloadedFile(db, transaction, fileId, request.File.FileName);
                        await InsertBlock1(db, transaction, swiftMessage, fileId);
                        await InsertBlock2(db, transaction, swiftMessage, fileId);
                        await InsertBlock3(db, transaction, swiftMessage, fileId);
                        await InsertBlock4(db, transaction, swiftMessage, fileId);
                        await InsertBlock5(db, transaction, swiftMessage, fileId);

                        transaction.Commit(); 
                    }    

                    db.Close();
                };

                return Result<Unit>.Success(Unit.Value);
            }

            private async Task InsertUloadedFile(IDbConnection db, IDbTransaction transaction, string fileId, string fileName)
            {
                var uploadFileQuery = @"INSERT INTO UploadedFiles (Id, Name, DateUploaded)
                            VALUES (@Id, @Name, @DateUploaded)";
                var parameters = new { Id = fileId, Name = fileName, DateUploaded = DateTime.Now.ToLongDateString() };
                await db.ExecuteAsync(uploadFileQuery, parameters, transaction, Constants.DefaultTransactionTimeoutInSeconds, CommandType.Text);
            }

            private async Task InsertBlock1(IDbConnection db, IDbTransaction transaction, IDictionary<string, string> swiftMessageBlocks, string fileId)
            {
                if (swiftMessageBlocks.ContainsKey(Constants.BasicHeaderBlock1Key))
                {
                    var block1Query = @"INSERT INTO BasicHeaderBlocks (FileId, Value) VALUES (@FileId, @Value)";
                    var block1parameters = new { FileId = fileId, Value = swiftMessageBlocks[Constants.BasicHeaderBlock1Key] };
                    await db.ExecuteAsync(block1Query, block1parameters, transaction, Constants.DefaultTransactionTimeoutInSeconds, CommandType.Text);
                }
            }
            private async Task InsertBlock2(IDbConnection db, IDbTransaction transaction, IDictionary<string, string> swiftMessageBlocks, string fileId)
            {
                if (swiftMessageBlocks.ContainsKey(Constants.ApplicationHeaderBlock2Key))
                {
                    var block1Query = @"INSERT INTO ApplicationHeaderBlocks (FileId, Value) VALUES (@FileId, @Value)";
                    var block1parameters = new { FileId = fileId, Value = swiftMessageBlocks[Constants.ApplicationHeaderBlock2Key] };
                    await db.ExecuteAsync(block1Query, block1parameters, transaction, Constants.DefaultTransactionTimeoutInSeconds, CommandType.Text);
                }
            }

            private async Task InsertBlock3(IDbConnection db, IDbTransaction transaction, IDictionary<string, string> swiftMessageBlocks, string fileId)
            {
                if (swiftMessageBlocks.ContainsKey(Constants.UserHeaderBlock3Key))
                {
                    var block1Query = @"INSERT INTO UserHeaderBlocks (FileId, Value) VALUES (@FileId, @Value)";
                    var block1parameters = new { FileId = fileId, Value = swiftMessageBlocks[Constants.UserHeaderBlock3Key] };
                    await db.ExecuteAsync(block1Query, block1parameters, transaction, Constants.DefaultTransactionTimeoutInSeconds, CommandType.Text);
                }
            }

            private async Task InsertBlock4(IDbConnection db, IDbTransaction transaction, IDictionary<string, string> swiftMessageBlocks, string fileId)
            {
                if (swiftMessageBlocks.ContainsKey(Constants.TextBlockBlock4Key))
                {
                    var block1Query = @"INSERT INTO TextBlocks (FileId, Value) VALUES (@FileId, @Value)";
                    var block1parameters = new { FileId = fileId, Value = swiftMessageBlocks[Constants.TextBlockBlock4Key] };
                    await db.ExecuteAsync(block1Query, block1parameters, transaction, Constants.DefaultTransactionTimeoutInSeconds, CommandType.Text);
                }
            }

            private async Task InsertBlock5(IDbConnection db, IDbTransaction transaction, IDictionary<string, string> swiftMessageBlocks, string fileId)
            {
                if (swiftMessageBlocks.ContainsKey(Constants.TrailerBlock5Key))
                {
                    var block1Query = @"INSERT INTO TrailerBlocks (FileId, Value) VALUES (@FileId, @Value)";
                    var block1parameters = new { FileId = fileId, Value = swiftMessageBlocks[Constants.TrailerBlock5Key] };
                    await db.ExecuteAsync(block1Query, block1parameters, transaction, Constants.DefaultTransactionTimeoutInSeconds, CommandType.Text);
                }
            }
        }                             
    }
}
