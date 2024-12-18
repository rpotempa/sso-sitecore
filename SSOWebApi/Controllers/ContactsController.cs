using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using SSOWebApi.Models;
using SSOWebApi.Services;

namespace SSOWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IDataServices  _dataServices;

        public ContactsController(IDataServices dataServices)
        {
            _dataServices = dataServices;
        }

        [HttpGet(Name = "GetContacts")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "read:contacts")]
        public IEnumerable<Contact> Get()
        {
            return _dataServices.GetContacts();
        }


    }
}