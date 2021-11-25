using GenericLoginAspNetMvc.Models;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GenericLoginAspNetMvc.Authorization
{
    // This class constantly retrieves claims every request but is not attached to cookie.
    public class CustomClaimsTransformer : IClaimsTransformation
    {
        private readonly ApplicationDbContext context;

        public CustomClaimsTransformer(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = (ClaimsIdentity)principal.Identity;

            // Retrieve custom user claims from Db and add them here

            if (principal.Identity.Name == "mjzamorasClaims")
                identity.AddClaim(new Claim("QueryCap", "10"));

            identity.AddClaim(new Claim("Server_LastModifiedOn", "DB_RETRIEVED_VALUE_SERVER"));

            return new ClaimsPrincipal(principal);
        }
    }
}
