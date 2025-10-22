using LinhLong.Application.Auth.Commands.Login;
using FluentValidation;

namespace LinhLong.Application.Validation
{
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Request.Username).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Request.Password).NotEmpty().MinimumLength(12);
        }
    }
}
