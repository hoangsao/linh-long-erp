using LinhLong.Application.Auth.DTOs;
using MediatR;

namespace LinhLong.Application.Auth.Queries.GetMe
{
    public sealed record GetMeQuery() : IRequest<UserInfoDto>;
}
