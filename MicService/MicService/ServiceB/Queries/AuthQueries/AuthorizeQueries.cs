﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Resillience.Util;
using Resillience.Util.ResillienceResult;
using ServiceB.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServiceB.Queries.AuthQueries
{
    public class AuthorizeQueries : IAuthorizeQueries
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly Appsettings _settings;
        public AuthorizeQueries(IHttpClientFactory httpClient, IOptions<Appsettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }
        public async Task<ResillienceResult<string>> GenerateTokenAsync(string access_token)
        {
            var result = new ResillienceResult<string>();
            if (string.IsNullOrEmpty(access_token))
            {
                result.IsFailed("access_token为空");
                return result;
            }
            var url = $"{GitHubConfig.API_User}?access_token={access_token}";
            using var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.14 Safari/537.36 Edg/83.0.478.13");
            var httpResponse = await client.GetAsync(url);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                result.IsFailed("access_token不正确");
                return result;
            }
            var content = await httpResponse.Content.ReadAsStringAsync();
            var user = content.DeJson<UserResponse>();
            if (user.IsNull())
            {
                result.IsFailed("未获取到用户数据");
                return result;
            }
            //if (user.Id != GitHubConfig.UserId)
            //{
            //    result.IsFailed("当前账号未授权");
            //    return result;
            //}
            var claims = new[] {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Email, user.Login),
                    new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(_settings.JWT.Expires)).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
                };
            var key = new SymmetricSecurityKey(_settings.JWT.SecurityKey.SerializeUtf8());
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                issuer: _settings.JWT.Domain,
                audience: _settings.JWT.Domain,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_settings.JWT.Expires),
                signingCredentials: creds);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            result.IsSuccess(token);
            return await Task.FromResult(result);
        }

        public async Task<ResillienceResult<string>> GetAccessTokenAsync(string code)
        {
            var result = new ResillienceResult<string>();
            if (string.IsNullOrEmpty(code))
            {
                result.IsFailed("code为空");
                return result;
            }
            var request = new AccessTokenRequest();
            var content = new StringContent($"code={code}&client_id={request.Client_ID}&redirect_uri={request.Redirect_Uri}&client_secret={request.Client_Secret}");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            using var client = _httpClient.CreateClient();
            var httpResponse = await client.PostAsync(GitHubConfig.API_AccessToken, content);
            var response = await httpResponse.Content.ReadAsStringAsync();
            if (response.StartsWith("access_token"))
                result.IsSuccess(response.Split("=")[1].Split("&").First());
            else
                result.IsFailed("code不正确");
            return result;
        }

        public async Task<ResillienceResult<string>> GetLoginAddressAsync()
        {
            var result = new ResillienceResult<string>();
            var request = new AuthorizeRequest();
            var address = string.Concat(new string[]
            {
                    GitHubConfig.API_Authorize,
                    "?client_id=", request.Client_ID,
                    "&scope=", request.Scope,
                    "&state=", request.State,
                    "&redirect_uri=", request.Redirect_Uri
            });
            result.IsSuccess(address);
            return await Task.FromResult(result);
        }
    }
}
