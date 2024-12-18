using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SampleMvcApp.Models;
using SampleMvcApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SampleMvcApp.Controllers
{
    public class AccountController : Controller
    {
        public async Task Login(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .WithAudience("https://localhost:7211/")
                .WithScope("openid profile email read:contacts read:todos")
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be whitelisted in 
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Authorize]
        public IActionResult Profile()
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            var idToken = HttpContext.GetTokenAsync("id_token").Result;
            var refreshToken = HttpContext.GetTokenAsync("refresh_token").Result;
            IEnumerable<Todo> todos = new List<Todo>();
            IEnumerable<Contact> contacts = new List<Contact>();
            IEnumerable<WeatherForcast> weatherForcasts = new List<WeatherForcast>();

         

            var client = new RestClient("https://localhost:7211/Todos");
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("authorization", "Bearer " + accessToken);
            var response = client.Execute(request);
            if (response.Content != null)
            {
                todos = JsonConvert.DeserializeObject<IEnumerable<Todo>>(response.Content);
            }
           


            client = new RestClient("https://localhost:7211/Contacts");
            request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("authorization", "Bearer " + accessToken);
            response = client.Execute(request);
            if (response.Content != null)
            {
                contacts = JsonConvert.DeserializeObject<IEnumerable<Contact>>(response.Content);
            }

            client = new RestClient("https://localhost:7211/WeatherForecast");
            request = new RestRequest();
            request.Method = Method.Get;
            response = client.Execute(request);
            if (response.Content != null)
            {
                weatherForcasts = JsonConvert.DeserializeObject<IEnumerable<WeatherForcast>>(response.Content);
            }

            return View(new UserProfileViewModel()
            {
                Name = User.Identity.Name,
                EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value,
                SecurityTokens = new List<SecToken>() 
                {
                    new SecToken(nameof(accessToken), accessToken),
                    new SecToken(nameof(idToken), idToken),
                    new SecToken(nameof(refreshToken), refreshToken)
                },
                Todos = todos,
                Contacts = contacts,
                WeatherForcasts = weatherForcasts
            });
        }


        /// <summary>
        /// This is just a helper action to enable you to easily see all claims related to a user. It helps when debugging your
        /// application to see the in claims populated from the Auth0 ID Token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Claims()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
