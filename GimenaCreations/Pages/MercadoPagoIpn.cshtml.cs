using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GimenaCreations.Pages;

public class MercadoPagoIpnModel : PageModel
{
    public IActionResult OnGet()
    {
        return new OkResult();
    }

    public IActionResult OnPost()
    {
        return new OkResult();
    }
}
