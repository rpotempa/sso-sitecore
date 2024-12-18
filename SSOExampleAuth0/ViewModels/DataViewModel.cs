using SampleMvcApp.Models;
using System.Collections.Generic;

namespace SampleMvcApp.ViewModels
{
    public class DataViewModel
    {
        public IEnumerable<Todo> Todos { get; set; }

        public IEnumerable<Contact> Contacts { get; set; }

        public IEnumerable<WeatherForcast> WeatherForcasts { get; set; }
    }
}
