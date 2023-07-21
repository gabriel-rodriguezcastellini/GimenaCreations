namespace GimenaCreations.PDF.DocumentModels;

public class PurchaseModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string RequestedBy { get; set; }
    public string Importance { get; set; }
    public string PaymentMethod { get; set; }
    public List<PurchaseItem> Items { get; set; }
}

public class PurchaseItem
{
    public int CatalogItemId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
