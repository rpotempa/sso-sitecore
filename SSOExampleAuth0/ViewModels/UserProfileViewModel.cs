using Microsoft.IdentityModel.Tokens;
using SampleMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleMvcApp.ViewModels
{
    public class UserProfileViewModel
    {
        public string EmailAddress { get; set; }

        public string Name { get; set; }

        public string ProfileImage { get; set; }

        public IEnumerable<SecToken> SecurityTokens { get; set; }

        public IEnumerable<Todo> Todos { get; set; }
        
        public IEnumerable<Contact> Contacts { get; set; }

        public IEnumerable<WeatherForcast> WeatherForcasts { get; set; }

    }
}
