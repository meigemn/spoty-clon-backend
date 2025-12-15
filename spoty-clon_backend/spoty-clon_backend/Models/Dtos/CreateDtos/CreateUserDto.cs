namespace spoty_clon_backend.Models.Dtos.CreateDtos
{
    public class CreateUserDto
    {
        public string UserName{ get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password{ get; set; } = string.Empty;
    }
}
