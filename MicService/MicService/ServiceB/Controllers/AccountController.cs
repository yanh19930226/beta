using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceB.Controllers
{
    //[Route("api/test")]
    //[ApiController]
    public class AccountController : ControllerBase
    {
        // GET: Account
        public ActionResult Auth()
        {
            return Ok();
        }
    }
}