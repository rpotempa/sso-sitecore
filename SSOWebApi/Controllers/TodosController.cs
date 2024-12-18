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
    public class TodosController : ControllerBase
    {
        private readonly IDataServices _dataServices;

        public TodosController(IDataServices dataServices)
        {
            _dataServices = dataServices;
        }

        [HttpGet(Name = "GetTodos")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "read:todos")]
        public IEnumerable<Todo> Get()
        {
            return  _dataServices.GetTodos();
        }
    }
}