﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.General;

[Table("Products")]
public class Product : Base<int>
{
    [StringLength(maximumLength: 8, MinimumLength = 2)]
    public required string Code { get; set; }

    [StringLength(maximumLength: 100, MinimumLength = 2)]
    public required string Name { get; set; }

    public required double Price { get; set; }

    public int Quantity { get; set; }

    [StringLength(maximumLength: 350)]
    public string? Description { get; set; }
    
    public bool IsActive { get; set; }
}