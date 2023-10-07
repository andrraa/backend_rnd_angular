using System;
using System.Collections.Generic;

namespace RnD.Angular.Backend.Models;

public partial class UserModel
{
    public string Userid { get; set; } = null!;

    public string? Name { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public bool? IsActive { get; set; }
}
