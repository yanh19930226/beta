using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.OAuthLogin.Core
{
    /// <summary>
    /// 授权提供程序
    /// </summary>
    public interface IAuthorizationProvider : IAuthorizationUrlProvider<AuthorizationParam>
    {
    }
}
