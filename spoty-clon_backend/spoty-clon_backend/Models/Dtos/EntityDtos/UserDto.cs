namespace spoty_clon_backend.Models.Dtos.EntityDtos
{
    public class UserDto
    {
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the primary key for this user. No es nullable, debe inicializarse.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>

        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets a email for the user.
        /// </summary>
        public string? Email { get; set; }

    }
}
