using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using User.Service.DBContext;
using User.Service.Extensions;
using User.Service.Models.User;

namespace User.Service.DTO.User
{
    public class UpdateUserDto
    {
        [FromRoute]
        public int Id { get; set; }

        [FromBody]
        public UserBody body { get; set; }
    }

    public class UserBody
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        public string Password { get; set; }

        public string ImageName { get; set; }
    }

    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {

        public readonly IDbConnection DB;

        public ModelValidator<UserModel> modelValidator = new ModelValidator<UserModel>();

        public UpdateUserValidator(IPGSQLContext context)
        {
            DB = context.DB;

            RuleFor(item => item.Id)
                .Must(IsIdExist)
                .WithMessage("Data not found in database");

            RuleFor(item => item.body.Username)
               .Must(IsUniqueUsername)
               .WithMessage("Username must be unique")
               .Must(NotContainSpace)
               .WithMessage("Username cannot contains spaces");
        }

        public bool IsUniqueUsername(UpdateUserDto user, string value)
        {
            return modelValidator.IsUniqueUpdate(DB, "username", value, user.Id);
        }

        public bool NotContainSpace(string value)
        {
            return modelValidator.NotContainSpace(value);
        }

        public bool IsIdExist(UpdateUserDto user, int value)
        {
            return modelValidator.IsDataExist(DB, "id", value.ToString());
        }
    }
}
