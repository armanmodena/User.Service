using System;
using User.Service.DTO.User;
using User.Service.Models.User;

namespace User.Service.Extensions.AsDtos.User
{
    [Serializable]
    public static class AsUserDto
    {
        public static UserDto AsUserNoPasswordDto(this UserModel user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                ImageName = user.ImageName
            };
        }
    }
}
