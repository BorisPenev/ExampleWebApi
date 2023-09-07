using Application.Users;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Users.Validators
{
    public class CreateCommandValidatorTests
    {
        private readonly Create.CommandValidator createCommandValidator;

        public CreateCommandValidatorTests()
        {
            createCommandValidator = new Create.CommandValidator();
        }

        [Fact]
        public async Task Given_CreateCommand_When_FirstName_IsEmpty_Then_CreateCommand_Fails()
        {
            // Arrange
            var command = new Create.Command()
            {
                LastName = "Last Name",
                Email = "test@test.com",
                Title = "Title",
                Password = "Password",
                ConfirmPassword = "Password"
            };

            // Act
            var response = await createCommandValidator.TestValidateAsync(command);

            // Assert
            response.ShouldHaveValidationErrorFor(x => x.FirstName).Only();
        }

        [Fact]
        public async Task Given_CreateCommand_When_LastName_IsEmpty_Then_CreateCommand_Fails()
        {
            // Arrange
            var command = new Create.Command()
            {
                FirstName = "First Name",
                Email = "test@test.com",
                Title = "Title",
                Password = "Password",
                ConfirmPassword = "Password"
            };

            // Act
            var response = await createCommandValidator.TestValidateAsync(command);

            // Assert
            response.ShouldHaveValidationErrorFor(x => x.LastName).Only();
        }

        [Fact]
        public async Task Given_CreateCommand_When_Email_IsEmpty_Then_CreateCommand_Fails()
        {
            // Arrange
            var command = new Create.Command()
            {
                FirstName = "First Name",
                LastName = "Last Name",
                Title = "Title",
                Password = "Password",
                ConfirmPassword = "Password"
            };

            // Act
            var response = await createCommandValidator.TestValidateAsync(command);

            // Assert
            response.ShouldHaveValidationErrorFor(x => x.Email).Only();
        }

        [Fact]
        public async Task Given_CreateCommand_When_Email_IsInvalid_Then_CreateCommand_Fails()
        {
            // Arrange
            var command = new Create.Command()
            {
                FirstName = "First Name",
                LastName = "Last Name",
                Title = "Title",
                Email = "test@test.com@",
                Password = "Password",
                ConfirmPassword = "Password"
            };

            // Act
            var response = await createCommandValidator.TestValidateAsync(command);

            // Assert
            response.ShouldHaveValidationErrorFor(x => x.Email).Only();
        }

        [Fact]
        public async Task Given_CreateCommand_When_Title_IsEmpty_Then_CreateCommand_Fails()
        {
            // Arrange
            var command = new Create.Command()
            {
                FirstName = "First Name",
                LastName = "Last Name",
                Email = "test@test.com",
                Password = "Password",
                ConfirmPassword = "Password"
            };

            // Act
            var response = await createCommandValidator.TestValidateAsync(command);

            // Assert
            response.ShouldHaveValidationErrorFor(x => x.Title).Only();
        }

        [Fact]
        public async Task Given_CreateCommand_When_Password_IsEmpty_Then_CreateCommand_Fails()
        {
            // Arrange
            var command = new Create.Command()
            {
                FirstName = "First Name",
                LastName = "Last Name",
                Title = "Title",
                Email = "test@test.com",
                ConfirmPassword = "Password"
            };

            // Act
            var response = await createCommandValidator.TestValidateAsync(command);

            // Assert
            response.ShouldHaveValidationErrorFor(x => x.Password);
            response.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
            response.ShouldNotHaveValidationErrorFor(x => x.FirstName);
            response.ShouldNotHaveValidationErrorFor(x => x.LastName);
            response.ShouldNotHaveValidationErrorFor(x => x.Title);
            response.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Given_CreateCommand_When_Password_IsShorterThanMinLength_Then_CreateCommand_Fails()
        {
            // Arrange
            var command = new Create.Command()
            {
                FirstName = "First Name",
                LastName = "Last Name",
                Email = "test@test.com",
                Title = "Title",
                Password = "12345",
                ConfirmPassword = "123456"
            };

            // Act
            var response = await createCommandValidator.TestValidateAsync(command);

            // Assert
            response.ShouldHaveValidationErrorFor(x => x.Password);
            response.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
            response.ShouldNotHaveValidationErrorFor(x => x.FirstName);
            response.ShouldNotHaveValidationErrorFor(x => x.LastName);
            response.ShouldNotHaveValidationErrorFor(x => x.Title);
            response.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Given_CreateCommand_When_ConfirmPassword_IsEmpty_Then_CreateCommand_Fails()
        {
            // Arrange
            var command = new Create.Command()
            {
                FirstName = "First Name",
                LastName = "Last Name",
                Email = "test@test.com",
                Title = "Title",
                Password = "Password",
            };

            // Act
            var response = await createCommandValidator.TestValidateAsync(command);

            // Assert
            response.ShouldHaveValidationErrorFor(x => x.ConfirmPassword).Only();
        }

        [Fact]
        public async Task Given_CreateCommand_When_ConfirmPassword_IsDiffrentFromPassword_Then_CreateCommand_Fails()
        {
            // Arrange
            var command = new Create.Command()
            {
                FirstName = "First Name",
                LastName = "Last Name",
                Email = "test@test.com",
                Title = "Title",
                Password = "Password",
                ConfirmPassword = "123456"
            };

            // Act
            var response = await createCommandValidator.TestValidateAsync(command);

            // Assert
            response.ShouldHaveValidationErrorFor(x => x.ConfirmPassword).Only();
        }
    }
}
