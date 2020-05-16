using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DockerApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace DockerApi.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : Controller
    {
        private ConDbContext _conContext;

        public TestController(ConDbContext conContext)
        {
            _conContext = conContext;
        }
        public IActionResult Index()
        {
            var res = _conContext.Users.ToList();
            return Ok(res);
        }
    }
}