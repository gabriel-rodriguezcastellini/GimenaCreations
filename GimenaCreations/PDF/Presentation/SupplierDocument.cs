using GimenaCreations.PDF.DocumentModels;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GimenaCreations.PDF.Presentation;

public class SupplierDocument : IDocument
{
    public List<SupplierModel> Suppliers { get; }

    public SupplierDocument(List<SupplierModel> suppliers)
    {
        Suppliers = suppliers;
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
        container.AlignCenter().Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text("Suppliers list").Style(TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium).Underline());
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(5);
            column.Item().Element(ComposeTable);
        });
    }

    void ComposeTable(IContainer container)
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
                header.Cell().Element(CellStyle).Text("Supplier ID");
                header.Cell().Element(CellStyle).Text("Company name");
                header.Cell().Element(CellStyle).Text("Supplier type");
                header.Cell().Element(CellStyle).Text("Contact");
                header.Cell().Element(CellStyle).Text("Contact phone");

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });

            // step 3
            foreach (var item in Suppliers)
            {
                table.Cell().Element(CellStyle).Text(item.Id.ToString());
                table.Cell().Element(CellStyle).Text(item.BusinessName);
                table.Cell().Element(CellStyle).Text(item.Type);
                table.Cell().Element(CellStyle).Text(item.ContactName);
                table.Cell().Element(CellStyle).Text(item.ContactPhone);

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.FontSize(10)).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }
}
