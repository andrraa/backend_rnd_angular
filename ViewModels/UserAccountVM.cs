namespace API.EmployeeManagement.ViewModels
{
    public class UserAccountVM
    {
        public int Id { get; set; }

        public int? FkEmployee { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}
