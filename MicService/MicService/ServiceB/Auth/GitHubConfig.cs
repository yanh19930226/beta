using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Auth
{
    public class GitHubConfig
    {

        /// <summary>
        /// GET请求，跳转GitHub登录界面，获取用户授权，得到code
        /// </summary>
        public static string API_Authorize = "https://github.com/login/oauth/authorize";
        /// <summary>
        /// POST请求，根据code得到access_token
        /// </summary>
        public static string API_AccessToken = "https://github.com/login/oauth/access_token";
        /// <summary>
        /// GET请求，根据access_token得到用户信息
        /// </summary>
        public static string API_User = "https://api.github.com/user";
        /// <summary>
        /// Github UserId
        /// </summary>
        public static int UserId = 1;
        /// Client ID
        /// </summary>
        public static string Client_ID = "472bb877b17286f8c53e";
        /// <summary>
        /// Client Secret
        /// </summary>
        public static string Client_Secret = "8b8f379d1df7118196b2ba682a762aed425bd96d";
        /// <summary>
        /// Authorization callback URL
        /// </summary>
        public static string Redirect_Uri = "http://localhost:5000/account/auth";
        /// <summary>
        /// Application name
        /// </summary>
        public static string ApplicationName = "yanhTest";
    }
}
