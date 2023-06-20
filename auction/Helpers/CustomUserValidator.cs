using auction.Models.Database.Entity;
using Microsoft.AspNetCore.Identity;

namespace auction.Helpers;

public class CustomUserValidator : UserValidator<ApplicationUser>
{
    public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        IdentityResult result = await base.ValidateAsync(manager, user);

        var errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

        if (user.UserName.Length < 3 || user.UserName.Length > 20)
        {
            errors.Add(new IdentityError
            {
                Description = "Username must be between 3 and 20 characters."
            });
        }

        return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
    }
}