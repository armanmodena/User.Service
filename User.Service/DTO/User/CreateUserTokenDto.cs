using System;

namespace User.Service.DTO.User
{
    public class CreateUserTokenDto
    {
        public int UserId { get; set; }

        public string RefreshToken { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiredAt { get; set; }
    }
}
