using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericLoginAspNetMvc.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("/error/unauthorized")]
        public IActionResult UnAuthorized()
        {
            return View("UnAuthorized");
        }
    }
}
