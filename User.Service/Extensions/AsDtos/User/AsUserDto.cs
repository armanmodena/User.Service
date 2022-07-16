using System;
using User.Service.DTO.User;
using User.Service.Models.User;

namespace User.Service.Extensions.AsDtos.User
{
    [Serializable]
    public static class AsUserDto
    {
        public static UserModel AsUserNoPasswordDto(this UserModel user)
        {
            return new UserModel
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
