// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using GimenaCreations.Data;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace GimenaCreations.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var user = await _userManager.FindByIdAsync(HttpContext.User?.Claims?.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            await _context.LoginLogoutAudits.AddAsync(new LoginLogoutAudit
            {
                ApplicationUserId = _userManager.GetUserId(HttpContext.User),
                LogoutTime = DateTime.UtcNow,
                Username = user.UserName,
                FullName = user.FirstName + " " + user.LastName
            });

            await _context.SaveChangesAsync();
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
