using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resillience.ResillienceApiResult;
using ServiceB.Queries.AuthQueries;

namespace ServiceB.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthorizeQueries _authorizeQueries;
        public AuthController(IAuthorizeQueries authorizeQueries)
        {
            _authorizeQueries = authorizeQueries;
        }
        /// <summary>
        /// 获取登录地址(GitHub)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("url")]
        public async Task<ApiResult<string>> GetLoginAddressAsync()
        {
            return await _authorizeQueries.GetLoginAddressAsync();
        }
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("access_token")]
        public async Task<ApiResult<string>> GetAccessTokenAsync(string code)
        {
            return await _authorizeQueries.GetAccessTokenAsync(code);
        }
        /// <summary>
        /// 登录成功，生成Token
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("token")]
        public async Task<ApiResult<string>> GenerateTokenAsync(string access_token)
        {
            return await _authorizeQueries.GenerateTokenAsync(access_token);
        }
    }
}