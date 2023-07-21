using System.ComponentModel.DataAnnotations.Schema;

namespace GimenaCreations.Entities;

public class File
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int OrderItemId { get; set; }
    public byte[] Content { get; set; }
    public OrderItem OrderItem { get; set; }

    [NotMapped]
    public IFormFile FormFile { get; set; }
}
