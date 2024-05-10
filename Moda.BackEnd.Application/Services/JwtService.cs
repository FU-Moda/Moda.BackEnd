using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Moda.BackEnd.Application.IRepositories;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.ConfigurationModel;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Common.Utils;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Moda.BackEnd.Application.Services
{
    public class JwtService : GenericBackendService, IJwtService
    {
        private readonly JWTConfiguration _jwtConfiguration;
        private readonly BackEndLogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Account> _userManager;

        public JwtService(IUnitOfWork unitOfWork,
        UserManager<Account> userManager,
        IServiceProvider serviceProvider,
        BackEndLogger logger)
            : base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _jwtConfiguration = Resolve<JWTConfiguration>()!;
        }

        public async Task<string> GenerateAccessToken(LoginRequestDto loginRequest)
        {
            try
            {
                var accountRepository = Resolve<IRepository<Account>>();
                var utility = Resolve<BackEnd.Common.Utils.Utility>();
                var user = await accountRepository!.GetByExpression(u =>
                    u!.Email.ToLower() == loginRequest.Email.ToLower());
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var claims = new List<Claim>
                         {
                        new(ClaimTypes.Email, loginRequest.Email),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new("AccountId", user.Id)
                         };
                        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.ToUpper())));
                        var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key!));
                        var token = new JwtSecurityToken(
                         _jwtConfiguration.Issuer,
                         _jwtConfiguration.Audience,
                         expires: utility!.GetCurrentDateInTimeZone().AddDays(1),
                         claims: claims,
                         signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                         );
                        return new JwtSecurityTokenHandler().WriteToken(token);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, this);
            }

            return string.Empty;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<TokenDto> GetNewToken(string refreshToken, string accountId)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var accessTokenNew = "";
                var refreshTokenNew = "";
                try
                {
                    var accountRepository = Resolve<IRepository<Account>>();
                    var utility = Resolve<BackEnd.Common.Utils.Utility>();

                    var user = await accountRepository!.GetByExpression(u => u!.Id.ToLower() == accountId);
                    if (user != null && user.RefreshToken == refreshToken)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        var claims = new List<Claim>
                        {
                            new(ClaimTypes.Email, user.Email),
                            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new("AccountId", user.Id)
                        };
                        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                        var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key!));
                        var token = new JwtSecurityToken
                          (
                              _jwtConfiguration.Issuer,
                              _jwtConfiguration.Audience,
                              expires: utility!.GetCurrentDateInTimeZone().AddDays(1),
                              claims: claims,
                              signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                          );

                        accessTokenNew = new JwtSecurityTokenHandler().WriteToken(token);
                        if (user.RefreshTokenExpiryTime <= utility.GetCurrentDateInTimeZone())
                        {
                            user.RefreshToken = GenerateRefreshToken();
                            user.RefreshTokenExpiryTime = utility.GetCurrentDateInTimeZone().AddDays(1);
                            refreshTokenNew = user.RefreshToken;
                        }
                        else
                        {
                            refreshTokenNew = refreshToken;
                        }
                    }


                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, this);
                }
                return new TokenDto { Token = accessTokenNew, RefreshToken = refreshTokenNew };
            }
        }
    }
}
