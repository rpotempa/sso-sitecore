using DockerExamples.SSO.Models;
using Microsoft.IdentityModel.Logging;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Controllers;
using Sitecore.Owin.Authentication.Extensions;
using System.Web.Mvc;

namespace DockerExamples.SSO.Controllers
{
    public class SSOLoginController : SitecoreController
    {
        public ActionResult GetLogin()
        {
            Log.Info("GetLogin", this);
            IdentityModelEventSource.ShowPII = true;
            var login = new Login();
            var targetHostName = "https://sso.dockerexamples.localhost";
            var redirectURI = "/identity/login/ss/PingOne";
            login.SignInUrl = targetHostName + redirectURI;
            login.SignOutUrl = Url.Action("LogoutUser");
            var user = Sitecore.Context.User;
            if (user != null && user.IsAuthenticated)
            {
                login.Email = user.Profile.Email;
                login.FullName = user.Profile.FullName;
            }

            return View(login);
        }

        public ActionResult LogoutUser()
        {           
            var idToken = ((System.Security.Claims.ClaimsIdentity)(Sitecore.Context.User.Identity)).GetIdToken();

            if (Sitecore.Context.User.IsAuthenticated)
            {
                Session.Abandon();
                Sitecore.Security.Authentication.AuthenticationManager.Logout();
            }

            return Redirect("https://dev-url.eu.auth0.com/oidc/logout?post_logout_redirect_uri=https://sso.dockerexamples.localhost/identity/postexternallogout&id_token_hint="+ idToken);
        }
    }
}