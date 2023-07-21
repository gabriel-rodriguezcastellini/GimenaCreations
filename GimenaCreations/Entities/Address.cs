using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GimenaCreations.Entities;

public class Address
{
    [PersonalData]
    [RegularExpression("\\b([A-Za-z ]*)(\\d*)", ErrorMessage = "Please, enter a valid street address.")]
    public string Street { get; set; }

    [PersonalData]
    [RegularExpression("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Please, enter a valid city name.")]
    public string City { get; set; }

    [PersonalData]
    [RegularExpression("[A-Z][a-z]+(?: +[A-Z][a-z]+)*", ErrorMessage = "Please, enter a valid state.")]
    public string State { get; set; }

    [PersonalData]
    [RegularExpression("[a-zA-Z]{2,}", ErrorMessage = "Please, enter a valid country.")]
    public string Country { get; set; }

    [PersonalData]
    [RegularExpression("^\\d{4,5}$", ErrorMessage = "Please, enter a valid zip code.")]
    public string ZipCode { get; set; }
}
