namespace GimenaCreations.Entities;

public class PurchaseHistoryItem
{
    public int Id { get; set; }
    public PurchaseHistory PurchaseHistory { get; set; }
    public int PurchaseHistoryId { get; set; }
    public string State { get; set; }
    public DateTime Date { get; set; }
}
