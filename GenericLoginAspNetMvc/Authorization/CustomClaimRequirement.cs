using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericLoginAspNetMvc.Authorization
{
    public class CustomClaimRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public string ClaimValue { get; }
        public CustomClaimRequirement(string claimType, string claimValue)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public class CustomClaimRequirementHandler : AuthorizationHandler<CustomClaimRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomClaimRequirement requirement)
            {
                var claim = context.User.Claims.Where(x => x.Type == requirement.ClaimType).FirstOrDefault();
                
                if(claim != null)
                {
                    if (requirement.ClaimValue == null) context.Succeed(requirement);
                    else
                    {
                        if (claim.Value == requirement.ClaimValue) context.Succeed(requirement);
                    }
                }

                return Task.CompletedTask;
            }
        }
    }

}
