using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.SwiftMessages
{
    public class SwiftMessageFileValidator : AbstractValidator<IFormFile>
    {
        public SwiftMessageFileValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Please upload a SWIFT message file!");
            RuleFor(x => x.Length).NotEmpty().WithMessage("Please make sure that the uploaded file is not Empty!");
            RuleFor(x => x.Length).InclusiveBetween(1, 2000);
            RuleFor(x => x.ContentType).Equal("text/plain");
        }
    }
}