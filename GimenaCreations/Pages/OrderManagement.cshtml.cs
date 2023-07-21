using GimenaCreations.Entities;
using GimenaCreations.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO.Compression;
using System.Security.Claims;

namespace GimenaCreations.Pages;

[Authorize]
public class OrderManagementModel : PageModel
{
    private readonly IOrderService _orderService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileService _fileService;
    private readonly IInvoiceService _invoiceService;

    public OrderManagementModel(IOrderService orderService, UserManager<ApplicationUser> userManager, IFileService fileService, IInvoiceService invoiceService)
    {
        _orderService = orderService;
        _userManager = userManager;
        _fileService = fileService;
        _invoiceService = invoiceService;
    }

    public ICollection<Order> Orders { get; set; }

    public async Task OnGetAsync()
    {
        Orders = await _orderService.GetAllOrdersAsync(_userManager.GetUserId(HttpContext.User));
    }

    public async Task<IActionResult> OnPostAsync(int orderId)
    {
        var files = await _fileService.GetFilesToDownloadAsync(orderId, _userManager.GetUserId(HttpContext.User));

        using var compressedFileStream = new MemoryStream();

        //Create an archive and store the stream in memory.
        using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, false))
        {
            foreach (var caseAttachmentModel in files)
            {
                //Create a zip entry for each attachment
                var zipEntry = zipArchive.CreateEntry(caseAttachmentModel.Name);

                //Get the stream of the attachment
                using var originalFileStream = new MemoryStream(caseAttachmentModel.Content);
                using var zipEntryStream = zipEntry.Open();
                //Copy the attachment stream to the zip entry stream
                await originalFileStream.CopyToAsync(zipEntryStream);
            }
        }

        return new FileContentResult(compressedFileStream.ToArray(), "application/zip") { FileDownloadName = "GimenaCreationsFiles.zip" };
    }

    public async Task<IActionResult> OnGetInvoiceAsync(int orderId)
    {
        await _invoiceService.GenerateInvoiceAsync(orderId, User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        return RedirectToPage("OrderManagement");
    }
}
