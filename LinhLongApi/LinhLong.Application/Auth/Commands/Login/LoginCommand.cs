using LinhLong.Application.Auth.DTOs;
using MediatR;

namespace LinhLong.Application.Auth.Commands.Login
{
    public sealed record LoginCommand(LoginRequestDto Request) : IRequest<LoginResultDto>;
}
