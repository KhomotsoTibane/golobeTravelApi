using FluentValidation;
using GolobeTravelApi.Models;

namespace GolobeTravelApi.Dtos.UserDto
{
    public class CreateUserRequest
    {
        public string CognitoId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
      
    }

    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(u => u.CognitoId)
                .NotEmpty().WithMessage("Cognito ID is required.");

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");
        }
    }
}
