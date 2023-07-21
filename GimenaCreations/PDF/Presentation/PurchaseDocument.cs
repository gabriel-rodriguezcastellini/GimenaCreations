using GimenaCreations.PDF.DocumentModels;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GimenaCreations.PDF.Presentation;

public class PurchaseDocument : IDocument
{
    public PurchaseModel Purchase { get; }

    public PurchaseDocument(PurchaseModel purchase)
    {
        Purchase = purchase;
    }

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);
                page.Header().AlignCenter().Element(ComposeHeader);                
                page.Content().Element(ComposeContent);

                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    void ComposeHeader(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Purchase #{Purchase.Id}").Style(titleStyle);
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Item().Element(ComposeTableHeader);
            column.Item().PaddingVertical(5).Text("Purchase order record detail");
            column.Item().Element(ComposeTable);
        });
    }

    void ComposeTableHeader(IContainer container)
    {
        container.Table(table =>
        {
            // step 1
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(20);
                columns.RelativeColumn(20);
                columns.RelativeColumn(20);
                columns.RelativeColumn(20);
                columns.RelativeColumn(20);
            });

            // step 2
            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Purchase ID");
                header.Cell().Element(CellStyle).Text("Date");
                header.Cell().Element(CellStyle).Text("Requested by");
                header.Cell().Element(CellStyle).Text("Importance");
                header.Cell().Element(CellStyle).Text("Payment method");

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderColor(Colors.Black);
                }
            });

            table.Cell().Element(CellStyle).Text(Purchase.Id.ToString());
            table.Cell().Element(CellStyle).Text(Purchase.Date.ToString());
            table.Cell().Element(CellStyle).Text(Purchase.RequestedBy);
            table.Cell().Element(CellStyle).Text(Purchase.Importance);
            table.Cell().Element(CellStyle).Text(Purchase.PaymentMethod);

            static IContainer CellStyle(IContainer container)
            {
                return container.DefaultTextStyle(x => x.FontSize(10)).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
            }            
        });
    }

    void ComposeTable(IContainer container)
    {        
        container.Table(table =>
        {
            // step 1
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(25);
                columns.RelativeColumn(25);
                columns.RelativeColumn(25);
                columns.RelativeColumn(25);                
            });            

            table.Header(header =>
            {                
                header.Cell().Element(CellStyle).Text("Product ID");
                header.Cell().Element(CellStyle).Text("Product name");
                header.Cell().Element(CellStyle).Text("Quantity");
                header.Cell().Element(CellStyle).Text("Price");

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black).Border(1).AlignCenter();
                }
            });

            // step 3
            foreach (var item in Purchase.Items)
            {
                table.Cell().Element(CellStyle).Text(item.CatalogItemId.ToString());
                table.Cell().Element(CellStyle).Text(item.ProductName);
                table.Cell().Element(CellStyle).Text(item.Quantity.ToString());
                table.Cell().Element(CellStyle).Text(item.Price.ToString());
            }

            static IContainer CellStyle(IContainer container)
            {
                return container.DefaultTextStyle(x => x.FontSize(10)).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5).AlignCenter();
            }
        });
    }
}
