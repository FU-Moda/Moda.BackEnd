using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.API.Middlewares;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;

namespace Moda.BackEnd.API.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("create-account")]
        [RemoveCacheAtrribute("account")]
        public async Task<AppActionResult> CreateAccount(SignUpRequestDto request)
        {
            return await _accountService.CreateAccount(request, false);

        }

        [HttpPost("create-shop-account")]
        [RemoveCacheAtrribute("account")]
        public async Task<AppActionResult> CreateShopAccount(SignUpShopRequestDto request)
        {
            return await _accountService.CreateShopAccount(request, false);
        }

        [HttpGet("get-all-account")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAllAccount(int pageIndex = 1, int pageSize = 10)
        {
            return await _accountService.GetAllAccount(pageIndex, pageSize);
        }

        [HttpGet("get-accounts-by-role-name/{roleName}/{pageIndex:int}/{pageSize:int}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAccountsByRoleName(string roleName, int pageIndex = 1, int pageSize = 10)
        {
            return await _accountService.GetAccountsByRoleName(roleName, pageIndex, pageSize);
        }

        [HttpPost("login")]
        [RemoveCacheAtrribute("account")]
        public async Task<AppActionResult> Login(LoginRequestDto request)
        {
            return await _accountService.Login(request);
        }

        [HttpGet("get-accounts-by-role-id/{roleId}/{pageIndex:int}/{pageSize:int}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAccountsByRoleId(Guid roleId, int pageIndex = 1, int pageSize = 10)
        {
            return await _accountService.GetAccountsByRoleId(roleId, pageIndex, pageSize);
        }

        [HttpPut("update-account")]
        [RemoveCacheAtrribute("account")]
        public async Task<AppActionResult> UpdateAccount(UpdateAccountRequestDto request)
        {
            return await _accountService.UpdateAccount(request);
        }

        [HttpPost("get-account-by-userId/{id}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAccountByUserId(string id)
        {
            return await _accountService.GetAccountByUserId(id);
        }

        [HttpPut("change-password")]
        [RemoveCacheAtrribute("account")]
        public async Task<AppActionResult> ChangePassword(ChangePasswordDto dto)
        {
            return await _accountService.ChangePassword(dto);
        }

        [HttpPost("get-new-token/{userId}")]
        [RemoveCacheAtrribute("account")]
        public async Task<AppActionResult> GetNewToken([FromBody] string refreshToken, string userId)
        {
            return await _accountService.GetNewToken(refreshToken, userId);
        }

        [HttpPut("forgot-password")]
        [RemoveCacheAtrribute("account")]
        public async Task<AppActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            return await _accountService.ForgotPassword(dto);
        }

        [HttpPut("active-account/{email}/{verifyCode}")]
        [RemoveCacheAtrribute("account")]
        public async Task<AppActionResult> ActiveAccount(string email, string verifyCode)
        {
            return await _accountService.ActiveAccount(email, verifyCode);
        }

        [HttpPost("send-email-forgot-password/{email}")]
        [RemoveCacheAtrribute("account")]
        public async Task<AppActionResult> SendEmailForgotPassword(string email)
        {
            return await _accountService.SendEmailForgotPassword(email);
        }

        [HttpPost("send-email-for-activeCode/{email}")]
        [RemoveCacheAtrribute("account")]
        public async Task<AppActionResult> SendEmailForActiveCode(string email)
        {
            return await _accountService.SendEmailForActiveCode(email);
        }

        [HttpPost("google-callback")]
        [RemoveCacheAtrribute("account")]
        public async Task<AppActionResult> GoogleCallBack([FromBody] string accessTokenFromGoogle)
        {
            return await _accountService.GoogleCallBack(accessTokenFromGoogle);
        }

        [RemoveCacheAtrribute("account")]
        [HttpGet("remove-cache")]
        public IActionResult RemoveCache()
        {
            return Ok();
        }
    }
}
