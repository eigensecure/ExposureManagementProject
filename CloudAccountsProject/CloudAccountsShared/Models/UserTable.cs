using System;
using System.Collections.Generic;

namespace CloudAccountsShared.Models;

public partial class UserTable
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Pass { get; set; } = null!;
}
