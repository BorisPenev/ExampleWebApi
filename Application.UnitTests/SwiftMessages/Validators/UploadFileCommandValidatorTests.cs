using Application.SwiftMessages;
using FluentValidation.TestHelper;

namespace Application.UnitTests.SwiftMessages.Validators
{
    public class UploadFileCommandValidatorTests
    {
        private readonly UploadFile.CommandValidator uploadFileCommandValidator;

        public UploadFileCommandValidatorTests()
        {
            uploadFileCommandValidator = new UploadFile.CommandValidator();
        }

        [Fact]
        public async Task Given_UploadFileCommand_When_File_IsEmpty_Then_UploadFileCommand_Fails()
        {
            // Arrange
            var file = TestHelpers.GenFormFile("SwiftMessages\\TestFiles\\EmptyTestFile.txt", "text/plain", null, "EmptyTestFile.txt");
            var command = new UploadFile.Command() { File = file };

            // Act
            var response = await uploadFileCommandValidator.TestValidateAsync(command);

            // Assert
            Assert.Equal(2, response.Errors.Count);
            Assert.Equal($"Please make sure that the uploaded file is not Empty!", response.Errors[0].ErrorMessage);
            Assert.Equal($"'Length' must be between 1 and 2000. You entered {file.Length}.", response.Errors[1].ErrorMessage);
            response.ShouldHaveValidationErrorFor(x => x.File.Length).Only();
        }

        [Fact]
        public async Task Given_UploadFileCommand_When_File_MimeTypeIsNotText_Then_UploadFileCommand_Fails()
        {
            // Arrange
            var file = TestHelpers.GenFormFile("SwiftMessages\\TestFiles\\TestFileWith1to2000Length.txt", "bla", null, "TestFileWith1to2000Length.txt");
            var command = new UploadFile.Command() { File = file };

            // Act
            var response = await uploadFileCommandValidator.TestValidateAsync(command);

            // Assert
            Assert.Single(response.Errors);
            Assert.Equal("'Content Type' must be equal to 'text/plain'.", response.Errors[0].ErrorMessage);
            response.ShouldHaveValidationErrorFor(x => x.File.ContentType).Only();
        }

        [Fact]
        public async Task Given_UploadFileCommand_When_File_HasExceededMaxLength_Then_UploadFileCommand_Fails()
        {
            // Arrange
            var file = TestHelpers.GenFormFile("SwiftMessages\\TestFiles\\TestFileWithMoreThan2000Length.txt", "text/plain", null, "TestFileWithMoreThan2000Length.txt");
            var command = new UploadFile.Command() { File = file };

            // Act
            var response = await uploadFileCommandValidator.TestValidateAsync(command);

            // Assert
            Assert.Single(response.Errors);
            Assert.Equal($"'Length' must be between 1 and 2000. You entered {file.Length}.", response.Errors[0].ErrorMessage);
            response.ShouldHaveValidationErrorFor(x => x.File.Length).Only();
        }
    }
}
