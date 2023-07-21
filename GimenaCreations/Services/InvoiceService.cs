using GimenaCreations.PDF.Presentation;
using QuestPDF.Fluent;
using System.Diagnostics;

namespace GimenaCreations.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IOrderService _orderService;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public InvoiceService(IOrderService orderService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        _orderService = orderService;
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task GenerateInvoiceAsync(int id, string userId)
    {
        var model = await _orderService.GetOrderByIdAsync(id, userId);
        
        var document = new InvoiceDocument(new PDF.DocumentModels.InvoiceModel
        {
            Comments = model.Description,
            CustomerAddress = new PDF.DocumentModels.Address
            {
                City = model.Address.City,
                State = model.Address.State,
                CompanyName = null,
                Email = model.ApplicationUser.Email,
                Phone = model.ApplicationUser.PhoneNumber,
                Street = model.Address.Street,
                FullName = model.ApplicationUser.FullName
            },
            InvoiceNumber = model.Id,
            IssueDate = model.Date,
            SellerAddress = new PDF.DocumentModels.Address
            {
                City = _configuration.GetValue<string>("Address:City"),
                CompanyName = _configuration.GetValue<string>("Address:CompanyName"),
                Email = _configuration.GetValue<string>("Address:Email"),
                Phone = _configuration.GetValue<string>("Address:Phone"),
                State = _configuration.GetValue<string>("Address:State"),
                Street = _configuration.GetValue<string>("Address:Street")
            },
            Items = model.Items.Select(x => new PDF.DocumentModels.OrderItem
            {
                Name = x.ProductName,
                Price = x.UnitPrice,
                Quantity = x.Units
            }).ToList()
        }, _webHostEnvironment);

        document.GeneratePdf("invoice.pdf");
        Process.Start("explorer.exe", "invoice.pdf");
    }
}
