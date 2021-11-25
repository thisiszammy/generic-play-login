using GenericLoginAspNetMvc.Interfaces;
using GenericLoginAspNetMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GenericLoginAspNetMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository userRepository;

        public HomeController(ILogger<HomeController> logger,
            IUserRepository userRepository)
        {
            _logger = logger;
            this.userRepository = userRepository;
        }

        [HttpGet("/register")]
        public IActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount([FromForm] CreateUserViewModel userViewModel)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    await userRepository.CreateUser(userViewModel.FirstName,
                        userViewModel.LastName,
                        userViewModel.Username,
                        userViewModel.Password);

                    TempData["RegisterSuccess"] = true;
                    return RedirectToAction("Login");

                }catch(Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View("CreateAccount", userViewModel);
                }

            }

            return View("CreateAccount");
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            ViewBag.RegisterSuccess = TempData["RegisterSuccess"];
            return View("Login");
        }

        [HttpPost("/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] AuthenticateUserViewModel authenticateUserViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await userRepository.AuthenticateUser(authenticateUserViewModel.Username, authenticateUserViewModel.Password, false, false);
                    return Json("Authentication Success");
                }catch(Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View("Login", authenticateUserViewModel);
                }
            }

            ViewBag.Error = "Authentication Failed";
            return View("Login", authenticateUserViewModel);
        }

    }
}
