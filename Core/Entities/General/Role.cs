﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.General;

public class Role : IdentityRole<int>
{
    [StringLength(maximumLength: 10, MinimumLength = 2)]
    public required string Code { get; set; }

    public bool IsActive { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}