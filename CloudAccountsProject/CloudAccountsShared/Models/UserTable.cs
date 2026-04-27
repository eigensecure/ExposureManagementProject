using System;
using System.Collections.Generic;

namespace CloudAccountsShared.Models;

public partial class UserTable
{
    public string UserId { get; set; } = null!;

    public string? PersonId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Upn { get; set; }

    public string? Email { get; set; }

    public string? ManagerUserId { get; set; }

    public string? Status { get; set; }

    public string? PhoneNumber { get; set; }

    public string? CountryCode { get; set; }

    public string? Country { get; set; }

    public string? Region { get; set; }

    public string? BusinessUnit { get; set; }

    public string? Category { get; set; }

    public string? Title { get; set; }

    public bool? AdminRole { get; set; }

    public bool? IsManager { get; set; }

    public string? AzureObjectId { get; set; }

    public string? GroupOf { get; set; }

    public string? SiteCode { get; set; }

    public string? Department { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public string? SegroupOf { get; set; }

    public string? BcdrgroupOf { get; set; }

    public string? RiskGroupOf { get; set; }
}
