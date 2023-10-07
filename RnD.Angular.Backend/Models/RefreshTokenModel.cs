using System;
using System.Collections.Generic;

namespace RnD.Angular.Backend.Models;

public partial class RefreshTokenModel
{
    public string UserId { get; set; } = null!;

    public string? TokenId { get; set; }

    public string? RefreshToken { get; set; }

    public bool? IsActive { get; set; }
}
