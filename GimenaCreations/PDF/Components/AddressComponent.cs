using GimenaCreations.PDF.DocumentModels;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace GimenaCreations.PDF.Components;

public class AddressComponent : IComponent
{
    private string Title { get; }
    private Address Address { get; }

    public AddressComponent(string title, Address address)
    {
        Title = title;
        Address = address;
    }

    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(2);

            column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();

            if (!string.IsNullOrWhiteSpace(Address.CompanyName))
            {
                column.Item().Text(Address.CompanyName);
            }

            if (!string.IsNullOrWhiteSpace(Address.FullName))
            {
                column.Item().Text(Address.FullName);
            }
            
            column.Item().Text(Address.Street);
            column.Item().Text($"{Address.City}, {Address.State}");
            column.Item().Text(Address.Email);
            column.Item().Text(Address.Phone);
        });
    }
}
