using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimenaCreations.Entities;

[Table(nameof(AuditEntry))]
public class AuditEntry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Display(Name = "Entity name")]
    public string EntityName { get; set; }

    [Display(Name = "Action type")]
    public string ActionType { get; set; }

    [Display(Name = "User ID")]
    public string ApplicationUserId { get; set; }

    public ApplicationUser User { get; set; }

    [Display(Name = "Date")]
    public DateTime TimeStamp { get; set; }

    [Display(Name = "Entity ID")]
    public string EntityId { get; set; }

    public Dictionary<string, object> Changes { get; set; }

    [NotMapped]
    // TempProperties are used for properties that are only generated on save, e.g. ID's
    public List<PropertyEntry> TempProperties { get; set; }
}
