using System.ComponentModel.DataAnnotations;

namespace Core.Common;

public class ExpressionFilter
{
    public string? PropertyName { get; set; }

    public object? Value { get; set; }

    public Comparison Comparison { get; set; }
}

public enum Comparison
{
    [Display(Name = "==")]
    Equal,

    [Display(Name = "<")]
    LessThan,

    [Display(Name = "<=")]
    LessThanOrEqual,

    [Display(Name = ">")]
    GreaterThan,

    [Display(Name = ">=")]
    GreaterThanOrEqual,

    [Display(Name = "!=")]
    NotEqual,

    [Display(Name = "Contains")]
    Contains,

    [Display(Name = "StartsWith")]
    StartsWith,

    [Display(Name = "EndsWith")]
    EndsWith,
}
