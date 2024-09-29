namespace CartMaster.Data.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UserDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class UserWithRole
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int RoleID {  get; set; }
        public string? RoleName { get; set; }
        public int CartID { get; set; }
    }
}
