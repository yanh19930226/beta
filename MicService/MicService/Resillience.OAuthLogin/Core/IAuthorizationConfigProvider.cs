using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resillience.OAuthLogin.Core
{
    /// <summary>
    /// 授权配置提供程序
    /// </summary>
    public interface IAuthorizationConfigProvider<TAuthorizationConfig>
        where TAuthorizationConfig : IAuthorizationConfig
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        Task<TAuthorizationConfig> GetConfigAsync();
    }
}
