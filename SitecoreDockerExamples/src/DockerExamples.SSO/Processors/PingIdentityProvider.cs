using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Sitecore.Abstractions;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Configuration;
using Sitecore.Owin.Authentication.Pipelines.IdentityProviders;
using Sitecore.Owin.Authentication.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DockerExamples.SSO.Processors
{
    public class PingIdentityProvider : IdentityProvidersProcessor
    {
        public PingIdentityProvider(FederatedAuthenticationConfiguration federatedAuthenticationConfiguration, ICookieManager cookieManager, BaseSettings settings) : base(federatedAuthenticationConfiguration, cookieManager, settings)
        {
        }

        protected override string IdentityProviderName
        {
            get { return "PingOne"; }
        }

        protected override void ProcessCore(IdentityProvidersArgs args)
        {
            Log.Info("ProcessCOre of PingOne", this);

            Assert.ArgumentNotNull(args, nameof(args));

            var identityProvider = GetIdentityProvider();
            var authenticationType = GetAuthenticationType();
            var saveSigninToken = identityProvider.TriggerExternalSignOut;

            var targetHostName = "https://sso.dockerexamples.localhost";

            var clientId = Settings.GetSetting("DockerExamples.SSO.ClientId");
            var clientSecret = Settings.GetSetting("DockerExamples.SSO.ClientSecret");
            var authority = Settings.GetSetting("DockerExamples.SSO.Authority");
            var redirectURI = Settings.GetSetting("DockerExamples.SSO.RedirectURI");
            var postLogoutRedirectURI = Settings.GetSetting("DockerExamples.SSO.PostLogoutRedirectURI");
            var scope = Settings.GetSetting("DockerExamples.SSO.Scope");
            var metadataAddressURI = Settings.GetSetting("DockerExamples.SSO.MetadataAddressURI");


            args.App.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            });

            args.App.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Caption = identityProvider.Caption,
                AuthenticationType = authenticationType,
                AuthenticationMode = AuthenticationMode.Passive,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Authority = authority,
                PostLogoutRedirectUri = $"{targetHostName}{postLogoutRedirectURI}",
                RedirectUri = $"{targetHostName}{redirectURI}",
                ResponseType = OpenIdConnectResponseType.Code,
                UseTokenLifetime = false,
                CookieManager = this.CookieManager,
                Scope = scope,
                MetadataAddress = $"{authority}{metadataAddressURI}",
                RedeemCode = true,
                SaveTokens = true,
                ResponseMode = OpenIdConnectResponseMode.Query,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = notification =>
                    {
                        Log.Info("SecurityTokenValidated", this);
                        var identity = notification.AuthenticationTicket.Identity;
                        foreach (var claimTransformationService in identityProvider.Transformations)
                        {
                            claimTransformationService.Transform(identity, new TransformationContext(FederatedAuthenticationConfiguration, identityProvider));
                        }

                        identity.AddClaim(new Claim("id_token", notification.ProtocolMessage.IdToken));
                        identity.AddClaim(new Claim("access_token", notification.ProtocolMessage.AccessToken));

                        notification.AuthenticationTicket = new AuthenticationTicket(identity, notification.AuthenticationTicket.Properties);

                        return Task.FromResult(0);
                    },
                    AuthenticationFailed = message =>
                    {
                        Log.Info("AuthenticationFailed", this);
                        return Task.FromResult(0);
                    },
                },
                TokenValidationParameters =
               {
                   SaveSigninToken = saveSigninToken,
                   ValidateIssuer = false
               }
            });

        }
    }
}