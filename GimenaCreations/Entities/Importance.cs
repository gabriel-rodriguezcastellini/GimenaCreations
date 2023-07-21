using System.ComponentModel.DataAnnotations;

namespace GimenaCreations.Entities;

public enum Importance
{
    [Display(Name = "Low")]
    Low,

    [Display(Name = "Normal")]
    Normal,

    [Display(Name = "Medium")]
    Medium,

    [Display(Name = "High")]
    High
}
