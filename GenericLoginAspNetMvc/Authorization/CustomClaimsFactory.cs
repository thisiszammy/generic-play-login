using GenericLoginAspNetMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GenericLoginAspNetMvc.Authorization
{
    // This class attaches user's claims to cookie (encrypted) upon initial sign-in. Any changes made to claims added here may require user to re-authenticate
    public class CustomClaimsFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;


        public CustomClaimsFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options,
            ApplicationDbContext context) : base(userManager, roleManager, options)
        {
            this.context = context;
        }

        protected async override Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // Retrieve Claims saved in Db and add them here (persists)

            if (user.UserName == "mjzamorasClaims")
                identity.AddClaim(new Claim("CustomClaim", "CustomValue"));

            identity.AddClaim(new Claim("Client_LastModifiedOn", "DB_RETRIEVED_VALUE_CLIENT"));


            return identity;
        }
    }
}
