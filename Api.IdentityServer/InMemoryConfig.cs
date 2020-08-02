using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.IdentityServer
{
    public class InMemoryConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResourceResources()
        {
            return new List<IdentityResource>
            {
                //必须要添加，否则报无效的scope错误
                new IdentityResources.OpenId(),
            };
        }

        /// <summary>
        /// api资源列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            //可访问的API资源(资源名，资源描述)
            return new List<ApiResource>
            {
                new ApiResource("Api_Catalog", "Api_Catalog"),
                new ApiResource("Api_Ordering", "Api_Ordering")
            };
        }

        /// <summary>
        /// 客户端列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client_Catalog", //访问客户端Id,必须唯一
                    //使用客户端授权模式，客户端只需要clientid和secrets就可以访问对应的api资源。
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "Api_Catalog", IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile }
                },
                new  Client
                {
                    ClientId = "client_Ordering",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    //这里使用的是通过用户名密码和ClientCredentials来换取token的方式. ClientCredentials允许Client只使用ClientSecrets来获取token. 这比较适合那种没有用户参与的api动作
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = { "Api_Ordering", IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile }
                }
            };
        }

        /// <summary>
        /// 指定可以使用 Authorization Server 授权的 Users（用户）
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestUser> Users()
        {
            return new[]
            {
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "cba",
                        Password = "abc",
                    }
            };
        }
    }
}
