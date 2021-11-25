using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericLoginAspNetMvc.Authorization
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(this AuthorizationPolicyBuilder builder, string claimType, string claimValue)
        {
            builder.AddRequirements(new CustomClaimRequirement(claimType, claimValue));
            return builder;
        }
    }
}
