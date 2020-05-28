﻿using Resillience.ResillienceApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Queries.AuthQueries
{
    public interface IAuthorizeQueries
    {
        /// <summary>
        /// 获取登录地址(GitHub)
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> GetLoginAddressAsync();
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<ApiResult<string>> GetAccessTokenAsync(string code);
        /// <summary>
        /// 登录成功，生成Token
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        Task<ApiResult<string>> GenerateTokenAsync(string access_token);
    }
}
