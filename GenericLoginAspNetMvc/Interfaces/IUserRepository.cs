using GenericLoginAspNetMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericLoginAspNetMvc.Interfaces
{
    public interface IUserRepository
    {
        public Task<string> CreateUser(string firstName, string lastName, string username, string password, string accountType, List<(string, string)> claims, List<string> roles);
        public Task<ApplicationUser> GetUserById(string Id);
        public Task AuthenticateUser(string username, string password, bool isPersistent, bool lockoutOnFailure);
        public Task CreateRole(string role);
        public Task AttachRoleToUser(ApplicationUser user, string role);
        public Task RemoveRoleFromUser(ApplicationUser user, string role);
        public Task SignOutUser();
    }
}
