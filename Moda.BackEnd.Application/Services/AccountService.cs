using AutoMapper;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Identity;
using Moda.BackEnd.Application.IRepositories;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.ConfigurationModel;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Common.Utils;
using Moda.BackEnd.Domain.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.Services
{
    public class AccountService : GenericBackendService, IAccountService
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IMapper _mapper;
        private readonly SignInManager<Account> _signInManager;
        private readonly TokenDto _tokenDto;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Account> _userManager;
        private readonly IEmailService _emailService;
        public AccountService(
            IRepository<Account> accountRepository,
        IUnitOfWork unitOfWork,
        UserManager<Account> userManager,
        SignInManager<Account> signInManager,
        IEmailService emailService,
        IMapper mapper,
        IServiceProvider serviceProvider
            ) : base(serviceProvider)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _tokenDto = new TokenDto();
            _mapper = mapper;
        }


        public async Task<AppActionResult> ActiveAccount(string email, string verifyCode)
        {
            var result = new AppActionResult();
            try
            {
                var user = await _accountRepository.GetByExpression(a =>
                    a!.Email == email && a.IsDeleted == false && a.IsVerified == false);
                if (user == null)
                    result = BuildAppActionResultError(result, "Tài khoản không tồn tại ");
                else if (user.VerifyCode != verifyCode)
                    result = BuildAppActionResultError(result, "Mã xác thực sai");

                if (!BuildAppActionResultIsError(result))
                {
                    user!.IsVerified = true;
                    user.VerifyCode = null;
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var result = new AppActionResult();

            try
            {
                if (await _accountRepository.GetByExpression(c =>
                        c!.Email == changePasswordDto.Email && c.IsDeleted == false) == null)
                    result = BuildAppActionResultError(result,
                        $"Tài khoản có email {changePasswordDto.Email} không tồn tại!");
                if (!BuildAppActionResultIsError(result))
                {
                    var user = await _accountRepository.GetByExpression(c =>
                        c!.Email == changePasswordDto.Email && c.IsDeleted == false);
                    var changePassword = await _userManager.ChangePasswordAsync(user!, changePasswordDto.OldPassword,
                        changePasswordDto.NewPassword);
                    if (!changePassword.Succeeded)
                        result = BuildAppActionResultError(result, "Thay đổi mật khẩu thất bại");
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> CreateAccount(SignUpRequestDto signUpRequest, bool isGoogle)
        {
            var result = new AppActionResult();
            try
            {
                if (await _accountRepository.GetByExpression(r => r!.UserName == signUpRequest.Email) != null)
                    result = BuildAppActionResultError(result, "Email hoặc username không tồn tại!");

                if (!BuildAppActionResultIsError(result))
                {
                    var emailService = Resolve<IEmailService>();
                    var verifyCode = string.Empty;
                    if (!isGoogle) verifyCode = Guid.NewGuid().ToString("N").Substring(0, 6);

                }
            } 
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var result = new AppActionResult();

            try
            {
                var user = await _accountRepository.GetByExpression(a =>
                    a!.Email == dto.Email && a.IsDeleted == false && a.IsVerified == true);
                if (user == null)
                    result = BuildAppActionResultError(result, "Tài khoản không tồn tại hoặc chưa được xác thực!");
                else if (user.VerifyCode != dto.RecoveryCode)
                    result = BuildAppActionResultError(result, "Mã xác thực sai!");

                if (!BuildAppActionResultIsError(result))
                {
                    await _userManager.RemovePasswordAsync(user!);
                    var addPassword = await _userManager.AddPasswordAsync(user!, dto.NewPassword);
                    if (addPassword.Succeeded)
                        user!.VerifyCode = null;
                    else
                        result = BuildAppActionResultError(result, "Thay đổi mật khẩu thất bại. Vui lòng thử lại");
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<string> GenerateVerifyCode(string email, bool isForForgettingPassword)
        {
            var code = string.Empty;

            var user = await _accountRepository.GetByExpression(a =>
                a!.Email == email && a.IsDeleted == false && a.IsVerified == isForForgettingPassword);

            if (user != null)
            {
                code = Guid.NewGuid().ToString("N").Substring(0, 6);
                user.VerifyCode = code;
            }

            await _unitOfWork.SaveChangesAsync();

            return code;
        }

        public async Task<AppActionResult> GetAccountByUserId(string id)
        {
            var result = new AppActionResult();
            try
            {
                var account = await _accountRepository.GetById(id);
                if (account == null) result = BuildAppActionResultError(result, $"Tài khoản với id {id} không tồn tại !");
                if (!BuildAppActionResultIsError(result)) result.Result = account;
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> GetAllAccount(int pageIndex, int pageSize)
        {
            var result = new AppActionResult();
            var list = await _accountRepository.GetAllDataByExpression(null, pageIndex, pageSize, null, false, null);

            var userRoleRepository = Resolve<IRepository<IdentityUserRole<string>>>();
            var roleRepository = Resolve<IRepository<IdentityRole>>();
            var listRole = await roleRepository!.GetAllDataByExpression(null, 1, 100, null, false, null);
            var listMap = _mapper.Map<List<AccountResponse>>(list.Items);
            foreach (var item in listMap)
            {
                var userRole = new List<IdentityRole>();
                var role = await userRoleRepository!.GetAllDataByExpression(a => a.UserId == item.Id, 1, 100, null, false, null);
                foreach (var itemRole in role.Items!)
                {
                    var roleUser = listRole.Items!.ToList().FirstOrDefault(a => a.Id == itemRole.RoleId);
                    if (roleUser != null) userRole.Add(roleUser);
                }

                item.Role = userRole;
            }

            result.Result =
                new PagedResult<AccountResponse>
                { Items = listMap, TotalPages = list.TotalPages };
            return result;
        }

        public async Task<AppActionResult> GetNewToken(string refreshToken, string userId)
        {
            var result = new AppActionResult();

            try
            {
                var user = await _accountRepository.GetById(userId);
                if (user == null)
                    result = BuildAppActionResultError(result, "Tài khoản không tồn tại");
                else if (user.RefreshToken != refreshToken)
                    result = BuildAppActionResultError(result, "Mã làm mới không chính xác");

                if (!BuildAppActionResultIsError(result))
                {
                    var jwtService = Resolve<IJwtService>();
                    result.Result = await jwtService!.GetNewToken(refreshToken, userId);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> GoogleCallBack(string accessTokenFromGoogle)
        {
            var result = new AppActionResult();
            try
            {
                var existingFirebaseApp = FirebaseApp.DefaultInstance;
                if (existingFirebaseApp == null)
                {
                    var config = Resolve<FirebaseAdminSDK>();
                    var credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(new
                    {
                        type = config!.Type,
                        project_id = config.Project_id,
                        private_key_id = config.Private_key_id,
                        private_key = config.Private_key,
                        client_email = config.Client_email,
                        client_id = config.Client_id,
                        auth_uri = config.Auth_uri,
                        token_uri = config.Token_uri,
                        auth_provider_x509_cert_url = config.Auth_provider_x509_cert_url,
                        client_x509_cert_url = config.Client_x509_cert_url
                    }));
                    var firebaseApp = FirebaseApp.Create(new AppOptions
                    {
                        Credential = credential
                    });
                }

                var verifiedToken = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(accessTokenFromGoogle);
                var emailClaim = verifiedToken.Claims.FirstOrDefault(c => c.Key == "email");
                var nameClaim = verifiedToken.Claims.FirstOrDefault(c => c.Key == "name");
                var name = nameClaim.Value.ToString();
                var userEmail = emailClaim.Value.ToString();

                if (userEmail != null)
                {
                    var user = await _accountRepository.GetByExpression(a => a!.Email == userEmail && a.IsDeleted == false);
                    if (user == null)
                    {
                        var resultCreate =
                            await CreateAccount(
                                new SignUpRequestDto
                                {
                                    Email = userEmail,
                                    FirstName = name!,
                                    Gender = true,
                                    LastName = string.Empty,
                                    Password = "Google123@",
                                    PhoneNumber = string.Empty
                                }, true);
                        if (resultCreate.IsSuccess)
                        {
                            var account = (Account)resultCreate.Result!;
                            result = await LoginDefault(userEmail, account);
                        }
                    }

                    result = await LoginDefault(userEmail, user);
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> Login(LoginRequestDto loginRequest)
        {
            var result = new AppActionResult();
            try
            {
                var user = await _accountRepository.GetByExpression(u =>
               u!.Email.ToLower() == loginRequest.Email.ToLower() && u.IsDeleted == false);
                if (user == null)
                    result = BuildAppActionResultError(result, $" {loginRequest.Email} này không tồn tại trong hệ thống");
                else if (user.IsVerified == false)
                    result = BuildAppActionResultError(result, "Tài khoản này chưa được xác thực !");
                var passwordSignIn =
              await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);
                if (!passwordSignIn.Succeeded) result = BuildAppActionResultError(result, "Đăng nhâp thất bại");
                if (!BuildAppActionResultIsError(result)) result = await LoginDefault(loginRequest.Email, user);
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> SendEmailForActiveCode(string email)
        {
            var result = new AppActionResult();

            try
            {
                var user = await _accountRepository.GetByExpression(a =>
                    a!.Email == email && a.IsDeleted == false && a.IsVerified == false);
                if (user == null) result = BuildAppActionResultError(result, "Tài khoản không tồn tại hoặc chưa xác thực");

                if (!BuildAppActionResultIsError(result))
                {
                    var emailService = Resolve<IEmailService>();
                    var code = await GenerateVerifyCode(user!.Email, false);
                    emailService!.SendEmail(email, SD.SubjectMail.VERIFY_ACCOUNT,
                        TemplateMappingHelper.GetTemplateOTPEmail(TemplateMappingHelper.ContentEmailType.VERIFICATION_CODE,
                            code, user.FirstName!));
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> SendEmailForgotPassword(string email)
        {
            var result = new AppActionResult();

            try
            {
                var user = await _accountRepository.GetByExpression(a =>
                    a!.Email == email && a.IsDeleted == false && a.IsVerified == true);
                if (user == null) result = BuildAppActionResultError(result, "Tài khoản không tồn tại hoặc chưa được xác thực");

                if (!BuildAppActionResultIsError(result))
                {
                    var emailService = Resolve<IEmailService>();
                    var code = await GenerateVerifyCode(user!.Email, true);
                    emailService?.SendEmail(email, SD.SubjectMail.PASSCODE_FORGOT_PASSWORD,
                        TemplateMappingHelper.GetTemplateOTPEmail(TemplateMappingHelper.ContentEmailType.FORGOTPASSWORD,
                            code, user.FirstName!));
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> UpdateAccount(UpdateAccountRequestDto accountRequest)
        {
            var result = new AppActionResult();
            try
            {
                var account =
                    await _accountRepository.GetByExpression(
                        a => a!.UserName.ToLower() == accountRequest.Email.ToLower());
                if (account == null)
                    result = BuildAppActionResultError(result, $"Tài khoản với email {accountRequest.Email} không tồn tại!");
                if (!BuildAppActionResultIsError(result))
                {
                    account!.FirstName = accountRequest.FirstName;
                    account.LastName = accountRequest.LastName;
                    account.PhoneNumber = accountRequest.PhoneNumber;
                    result.Result = await _accountRepository.Update(account);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        public async Task<AppActionResult> VerifyLoginGoogle(string email, string verifyCode)
        {
            var result = new AppActionResult();
            try
            {
                var user = await _accountRepository.GetByExpression(u =>
                    u!.Email.ToLower() == email.ToLower() && u.IsDeleted == false);
                if (user == null)
                    result = BuildAppActionResultError(result, $"Email này không tồn tại");
                else if (user.IsVerified == false)
                    result = BuildAppActionResultError(result, "Tài khoản này chưa xác thực !");
                else if (user.VerifyCode != verifyCode)
                    result = BuildAppActionResultError(result, "Mã xác thực sai!");

                if (!BuildAppActionResultIsError(result))
                {
                    result = await LoginDefault(email, user);
                    user!.VerifyCode = null;
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }

        private async Task<AppActionResult> LoginDefault(string email, Account? user)
        {
            var result = new AppActionResult();

            var jwtService = Resolve<IJwtService>();
            var utility = Resolve<Utility>();
            var token = await jwtService!.GenerateAccessToken(new LoginRequestDto { Email = email });

            if (user!.RefreshToken == null)
            {
                user.RefreshToken = jwtService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = utility!.GetCurrentDateInTimeZone().AddDays(1);
            }

            if (user.RefreshTokenExpiryTime <= utility!.GetCurrentDateInTimeZone())
            {
                user.RefreshTokenExpiryTime = utility.GetCurrentDateInTimeZone().AddDays(1);
                user.RefreshToken = jwtService.GenerateRefreshToken();
            }

            _tokenDto.Token = token;
            _tokenDto.RefreshToken = user.RefreshToken;
            result.Result = _tokenDto;
            await _unitOfWork.SaveChangesAsync();

            return result;
        }
    }
}
