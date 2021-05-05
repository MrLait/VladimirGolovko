using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
////using Microsoft.AspNetCore.Authentication.Cookies;
////using Microsoft.AspNetCore.Authentication.JwtBearer;
////using Microsoft.Extensions.Configuration;
////using Microsoft.Extensions.DependencyInjection;

namespace TicketManagement.WebMVC.Extencions
{
    public static class CustomAuthentication
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var identityUrl = configuration.GetValue<string>("IdentityUrl");
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddOpenIdConnect(options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authority = identityUrl.ToString();
                    options.RequireHttpsMetadata = false;
                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.SaveTokens = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "groups",
                        ValidateIssuer = true,
                    };
                    ////var identityUrl = configuration.GetValue<string>("IdentityUrl");
                    ////var callBackUrl = configuration.GetValue<string>("CallBackUrl");
                    ////var sessionCookieLifetime = configuration.GetValue("SessionCookieLifetimeMinutes", 1);

                    ////services.AddAuthentication(options =>
                    ////{
                    ////    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    ////    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    ////})
                    ////.AddCookie(setup => setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
                    ////.AddOpenIdConnect(options =>
                    ////{
                    ////    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    ////    options.Authority = identityUrl.ToString();
                    ////    options.SignedOutRedirectUri = callBackUrl.ToString();
                    ////    options.ClientId = "mvc";
                    ////    options.ClientSecret = "secret";
                    ////    options.ResponseType = "code id_token";
                    ////    options.SaveTokens = true;
                    ////    options.GetClaimsFromUserInfoEndpoint = true;
                    ////    options.RequireHttpsMetadata = false;
                    ////    options.Scope.Add("openid");
                    ////    options.Scope.Add("profile");
                    ////    options.Scope.Add("orders");
                    ////    options.Scope.Add("basket");
                    ////    options.Scope.Add("webshoppingagg");
                    ////    options.Scope.Add("orders.signalrhub");
                });

            return services;
        }
    }
}
