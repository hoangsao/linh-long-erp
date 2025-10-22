using LinhLong.Application.Auth.DTOs;
using MediatR;

namespace LinhLong.Application.Auth.Commands.Refresh
{
    public sealed record RefreshCommand(string RefreshToken) : IRequest<LoginResultDto>;
}
