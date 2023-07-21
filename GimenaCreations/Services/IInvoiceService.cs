namespace GimenaCreations.Services;

public interface IInvoiceService
{
    Task GenerateInvoiceAsync(int id, string userId);
}
