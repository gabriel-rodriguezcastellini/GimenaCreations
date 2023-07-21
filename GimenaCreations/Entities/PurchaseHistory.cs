namespace GimenaCreations.Entities;

public class PurchaseHistory
{
    public int Id { get; set; }
    public int PurchaseId { get; set; }
    public Purchase Purchase { get; set; }
    public List<PurchaseHistoryItem> Items { get; set; }
}
