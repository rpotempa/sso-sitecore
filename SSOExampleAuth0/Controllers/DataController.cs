using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SampleMvcApp.Models;
using SampleMvcApp.ViewModels;
using System.Collections.Generic;

namespace SampleMvcApp.Controllers
{
    public class DataController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<Todo> todos = new List<Todo>();
            IEnumerable<Contact> contacts = new List<Contact>();
            IEnumerable<WeatherForcast> weatherForcasts = new List<WeatherForcast>();

            var client = new RestClient("https://your_url.eu.auth0.com/oauth/token");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"clientid\",\"client_secret\":\"client-secret\",\"audience\":\"https://localhost:7211/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response.Content);

            client = new RestClient("https://localhost:7211/Todos");
            request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("authorization", "Bearer " + tokenResponse.AccessToken);
            response = client.Execute(request);
            if (response.Content != null)
            {
                todos = JsonConvert.DeserializeObject<IEnumerable<Todo>>(response.Content);
            }



            client = new RestClient("https://localhost:7211/Contacts");
            request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("authorization", "Bearer " + tokenResponse.AccessToken);
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

            return View(new DataViewModel()
            {
                Contacts = contacts,
                Todos = todos,  
                WeatherForcasts = weatherForcasts
            });
        }
    }
}
