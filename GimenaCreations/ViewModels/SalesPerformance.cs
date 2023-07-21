namespace GimenaCreations.ViewModels;

public class SalesPerformance
{
    public UniquePurchase UniquePurchase { get; set; } = new();
    public Quantity Quantity { get; set; } = new();
    public ProductRevenue ProductRevenue { get; set; } = new();
}

public class ProductRevenue : Change
{

}

public class Quantity : Change
{

}

public class UniquePurchase : Change
{

}

public class Change
{
    public decimal Value { get; set; }
    public decimal PercentageChange { get; set; }
    public decimal AbsoluteChange { get; set; }
    public Sign Sign { get; set; }
    public string Text => Sign == Sign.Positive ? "text-success" : "text-danger";
    public string Arrow => Sign == Sign.Positive ? "fa-caret-up" : "fa-caret-down";
}

public enum Sign
{
    Positive,
    Negative
}