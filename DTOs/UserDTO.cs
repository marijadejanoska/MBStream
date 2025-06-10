namespace MBStream.DTOs
{
    public class UserDTO
    {
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
    }

    public class UserResponseDTO : UserDTO
    {
        public int UserId { get; set; }
    }
}
