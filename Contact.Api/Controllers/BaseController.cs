using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.Api.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers
{
    public class BaseController : Controller
    {
        protected UserIdentity UserIdentity => new UserIdentity { UserId = 1};
    }
}