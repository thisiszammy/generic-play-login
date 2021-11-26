using GenericLoginAspNetMvc.Interfaces;
using GenericLoginAspNetMvc.Models;
using GenericLoginAspNetMvc.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GenericLoginAspNetMvc.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext,
            ILogger<UserRepository> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task AttachRoleToUser(ApplicationUser user, string role)
        {
            var taskAddToRole = await userManager.AddToRoleAsync(user, role);

            if (!taskAddToRole.Succeeded)
            {
                string error_message = "Error Attaching Role to User : " + string.Join(",", taskAddToRole.Errors.Select(x => x.Description).ToList());
                logger.LogError(error_message, user, role);
                throw new Exception(error_message);
            }
            else
            {
                logger.LogInformation("Successfully Attached Role to User", role, user);
            }

        }

        public async Task AuthenticateUser(string username, string password, bool isPersistent, bool lockoutOnFailure)
        {
            ValidationUtil.UserRepository_AuthenticateUser(username, password);

            var taskAuthenticate = await signInManager.PasswordSignInAsync(username, password,isPersistent, lockoutOnFailure);

            if (!taskAuthenticate.Succeeded)
                throw new Exception("Authentication Failed!");
        
        }

        public async Task CreateRole(string role)
        {
            var newRole = new IdentityRole(role);

            if(!dbContext.Roles.Any(x=>x.Name == role))
            {
                var taskCreateRole = await roleManager.CreateAsync(newRole);

                if (!taskCreateRole.Succeeded)
                {
                    string error_message = "Error Creating Role : " + string.Join(",", taskCreateRole.Errors.Select(x => x.Description).ToList());
                    logger.LogError( error_message, newRole);
                    throw new Exception(error_message);
                }
                logger.LogInformation("Successfully Created Role : " + role, newRole);
            }
        }

        public async Task<string> CreateUser(string firstName, string lastName, string username, string password, string accountType,
            List<(string, string)> claims, List<string> roles)
        {
            ValidationUtil.UserRepository_CreateUser(firstName, lastName, username, password, accountType);

            AccountTypeEnum accountTypeEnum;

            if (!Enum.TryParse(accountType, out accountTypeEnum))
                throw new Exception("Invalid Account Type");

            ApplicationUser applicationUser = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = username,
                LockoutEnabled = true,
                AccessFailedCount = 5,
                AccountType = accountTypeEnum,
                AuthorityChangedTimestamp = DateTime.Now
            };


            var taskCreateUser = await userManager.CreateAsync(applicationUser, password);

            await CreateRole(accountTypeEnum.ToString());
            await AttachRoleToUser(applicationUser, accountTypeEnum.ToString());

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

        public async Task RemoveRoleFromUser(ApplicationUser user, string role)
        {
            var removeRoleFromUser = await userManager.RemoveFromRoleAsync(user, role);
            if (!removeRoleFromUser.Succeeded)
            {
                string error_message = "Error removing Role from User : " + string.Join(",", removeRoleFromUser.Errors.Select(x => x.Description).ToList());
                logger.LogError(error_message, user, role);
                throw new Exception(error_message);
            }
            else
            {
                logger.LogInformation("Successfully Removed Role From User", user, role);
            }
        }

        public async Task SignOutUser()
        {
            await signInManager.SignOutAsync();
        }

    }
}
