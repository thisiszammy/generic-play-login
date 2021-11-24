using GenericLoginAspNetMvc.Interfaces;
using GenericLoginAspNetMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericLoginAspNetMvc.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UserRepository> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public async Task AuthenticateUser(string username, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var taskAuthenticate = await signInManager.PasswordSignInAsync(username, password,isPersistent, lockoutOnFailure);

            if (!taskAuthenticate.Succeeded)
                throw new Exception("Authentication Failed!");
        
        }

        public async Task<string> CreateUser(string firstName, string lastName, string username, string password)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = username,
                LockoutEnabled = true,
                AccessFailedCount = 5
            };

            var taskCreateUser = await userManager.CreateAsync(applicationUser, password);

            if (!taskCreateUser.Succeeded)
            {
                string message = string.Join(',', taskCreateUser.Errors.Select(x => x.Description).ToList());
                logger.Log(LogLevel.Error, $"Error creating user: " + message);
                throw new Exception(message);
            }

            logger.Log(LogLevel.Information, $"Created User `{applicationUser.Id}`", applicationUser);

            return applicationUser.Id;
        }

        public async Task<ApplicationUser> GetUserById(string Id)
        {
            return await userManager.FindByIdAsync(Id);
        }
    }
}
