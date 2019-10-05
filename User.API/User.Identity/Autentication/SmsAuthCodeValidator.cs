using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Identity.Services;

namespace User.Identity.Autentication
{
    public class SmsAuthCodeValidator : IExtensionGrantValidator
    {
        public string GrantType => "sms_auth_code";

        private readonly IAuthCodeService _authCodeService;
        private readonly IUserService _userService;
        public  SmsAuthCodeValidator(IAuthCodeService authCodeService, IUserService userService)
        {
            _authCodeService = authCodeService;
            _userService = userService;
        }
        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var code = context.Request.Raw["auth_code"];
            var ErrorResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(code))
            {
                context.Result = ErrorResult;
                return;
            }
            if (!_authCodeService.Validate(phone, code))
            {
                context.Result = ErrorResult;
                return;
            }
            var userId = await _userService.CheckOrCreate(phone);
            if (userId <= 0)
            {
                context.Result = ErrorResult;
                return;
            }
            context.Result = new GrantValidationResult(userId.ToString(), GrantType);
        }
    }
}
