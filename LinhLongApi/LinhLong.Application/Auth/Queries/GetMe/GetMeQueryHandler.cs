using LinhLong.Application.Auth.DTOs;
using LinhLong.Application.Interfaces;
using MediatR;

namespace LinhLong.Application.Auth.Queries.GetMe
{
    public sealed class GetMeQueryHandler : IRequestHandler<GetMeQuery, UserInfoDto>
    {
        private readonly ICurrentUser _current;

        public GetMeQueryHandler(ICurrentUser current) => _current = current;

        public Task<UserInfoDto> Handle(GetMeQuery request, CancellationToken ct)
        {
            if (_current.UserId is null)
                throw new UnauthorizedAccessException();

            return Task.FromResult(new UserInfoDto(_current.UserId.Value, _current.UserName ?? "", _current.Roles));
        }
    }
}
