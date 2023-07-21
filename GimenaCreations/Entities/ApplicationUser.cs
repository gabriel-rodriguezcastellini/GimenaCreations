using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimenaCreations.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<Order> Orders { get; set; }

    [PersonalData, Display(Name = "First name")]
    public string FirstName { get; set; }

    [PersonalData, Display(Name = "Last name")]
    public string LastName { get; set; }

    [PersonalData]
    public Address Address { get; set; }

    public bool Active { get; set; }

    public DateTime DateTimeAdd { get; set; }

    [NotMapped]
    public IList<string> Roles { get; set; } = new List<string>();

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}