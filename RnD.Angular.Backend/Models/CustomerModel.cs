using System;
using System.Collections.Generic;

namespace RnD.Angular.Backend.Models;

public partial class CustomerModel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? CreditLimit { get; set; }
}
