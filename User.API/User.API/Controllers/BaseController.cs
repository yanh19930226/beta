using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.API.Dto;

namespace User.API.Controllers
{
    public class BaseController : Controller
    {
        protected UserIdentity UserIdentity
        {
            get
            {
                var identity = new UserIdentity();
                identity.UserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
                identity.Name = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
                identity.Title = User.Claims.FirstOrDefault(c => c.Type == "title").Value;
                identity.Avatar = User.Claims.FirstOrDefault(c => c.Type == "avatar").Value;
                identity.Company = User.Claims.FirstOrDefault(c => c.Type == "company").Value;
                return identity;
            }
        }
    }
}