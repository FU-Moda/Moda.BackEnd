using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.IServices
{
    public interface IJwtService
    {
        string GenerateRefreshToken();

        Task<string> GenerateAccessToken(LoginRequestDto loginRequest);

        Task<TokenDto> GetNewToken(string refreshToken, string accountId);
    }
}
