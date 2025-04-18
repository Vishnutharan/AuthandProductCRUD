namespace AuthandProductCRUD.Model
{
    public class User
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; } // In a real-world scenario, passwords should be hashed
        public string? Role { get; set; } // Add Role to indicate user/admin role
    }
}
