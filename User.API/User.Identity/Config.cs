using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity
{
    public class Config
    {
        //获取资源
        public static IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>{
               new IdentityResources.OpenId(),
               new IdentityResources.Profile(),
               new IdentityResources.Email(),
           };
        }
        //获取资源
        public static IEnumerable<ApiResource> GetResource()
        {
            return new List<ApiResource>{
               new ApiResource("gateway_api","gateway service"),
               new ApiResource("contact_api","contact service"),
               new ApiResource("user_api","user service"),
               new ApiResource("project_api","project service"),
               new ApiResource("recommend_api","recommend service")
           };
        }
        //获取客户端
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>{
               new Client{
                   ClientId="android",
                    ClientSecrets={
                       new Secret("secret".Sha256())
                       },
                   RefreshTokenExpiration=TokenExpiration.Sliding,
                   AllowOfflineAccess=true,
                   RequireClientSecret=false,
                   AllowedGrantTypes=new List<string>{"sms_auth_code" },
                   AlwaysIncludeUserClaimsInIdToken=true,
                   AllowedScopes={
                       "gateway_api",
                       "contact_api",
                        "user_api",
                        "project_api",
                        "recommend_api",
                       IdentityServerConstants.StandardScopes.Profile,
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.OfflineAccess,
                   }
               }
           };
        }
    }
}
