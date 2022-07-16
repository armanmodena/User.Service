using System.ComponentModel.DataAnnotations;
using System.Data;
using FluentValidation;
using User.Service.DBContext;
using User.Service.Extensions;
using User.Service.Models.User;

namespace User.Service.DTO.User
{
    public class CreateUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        public string Password { get; set; }

        public string ImageName { get; set; }
    }

    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {

        public readonly IDbConnection DB;

        public ModelValidator<UserModel> modelValidator = new ModelValidator<UserModel>();

        public CreateUserValidator(IPGSQLContext context)
        {
            DB = context.DB;

            RuleFor(item => item.Username)
               .Must(CheckPassword)
               .WithMessage("Password must be set for not modena email account")
               .Must(IsUniqueUsername)
               .WithMessage("Username must be unique")
               .Must(NotContainSpace)
               .WithMessage("Username cannot contains spaces");
        }

        public bool IsUniqueUsername(string value)
        {
            return modelValidator.IsUnique(DB, "username", value);
        }

        public bool NotContainSpace(string value)
        {
            return modelValidator.NotContainSpace(value);
        }

        public bool CheckPassword(CreateUserDto user, string value)
        {
            if(!user.Username.Contains("@modena.com") && string.IsNullOrEmpty(user.Password))
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
