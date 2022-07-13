using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.Service.DTO.User;
using User.Service.Extensions;
using User.Service.Extensions.AsDtos.User;
using User.Service.Models.User;
using User.Service.Services.Interfaces;
using User.Service.Utils;

namespace User.Service.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : BaseController
    {

        private readonly IUserService UserService;
        private readonly IUserTokenService UserTokenService;

        public object HaslUtil { get; private set; }

        public UserController(
          IUserService userService,
          IUserTokenService userTokenService)
        {
            UserService = userService;
            UserTokenService = userTokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string select, [FromQuery] string search, [FromQuery] string filterAnd, [FromQuery] string filterOr,
            [FromQuery] string filterOut, [FromQuery] string order = "id", [FromQuery] string direction = "asc", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var users = await UserService.GetAll(select, search, filterAnd, filterOr, filterOut, order, direction, page, pageSize);
            return users.Item1 != null ? HttpResponse(200, users.Item1) : HttpResponse(400, users.Item2.message);
        }

        [HttpGet("with-token")]
        public async Task<IActionResult> GetAllWithToken([FromQuery] string select, [FromQuery] string search, [FromQuery] string filterAnd, [FromQuery] string filterOr,
           [FromQuery] string filterOut, [FromQuery] string order = "id", [FromQuery] string direction = "asc", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var users = await UserService.GetAllWithToken(select, search, filterAnd, filterOr, filterOut, order, direction, page, pageSize);
            return users.Item1 != null ? HttpResponse(200, users.Item1) : HttpResponse(400, users.Item2.message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await UserService.Get(id);

                    if (user is null) 
                        return HttpResponse(404, "User not found");

                    return HttpResponse(200, user.AsUserNoPasswordDto());
                }
                catch (Exception ex)
                {
                    return ErrorResponse(ex);
                }
            }
            return BadRequest();
        }

        [HttpGet("{username}/username")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await UserService.GetByUsername(username);

                    if (user is null)
                        return HttpResponse(404, "User not found");

                    return HttpResponse(200, user);
                }
                catch (Exception ex)
                {
                    return ErrorResponse(ex);
                }
            }
            return BadRequest();
        }

        [HttpGet("{user_id}/token")]
        public async Task<IActionResult> GetUserTokenById(int user_id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userToken = await UserTokenService.GetByUserId(user_id);
                    if (userToken is null) 
                        return HttpResponse(404, $"Token with User ID: {user_id} not found");

                    return HttpResponse(200, userToken);
                }
                catch (Exception ex)
                {
                    return ErrorResponse(ex);
                }
            }
            return BadRequest();
        }

        [HttpGet("{user_id}/token/{refresh_token}")]
        public async Task<IActionResult> GetUserToken(int user_id, string refresh_token)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userToken = await UserTokenService.GetUserToken(user_id, refresh_token);
                    if (userToken is null)
                        return HttpResponse(404, $"Token not found");

                    return HttpResponse(200, userToken);
                }
                catch (Exception ex)
                {
                    return ErrorResponse(ex);
                }
            }
            return BadRequest();
        }

        [HttpPost("token")]
        public async Task<IActionResult> CreateUserToken([FromBody] CreateUserTokenDto userToken)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newUserToken = new UserToken()
                    {
                        UserId = userToken.UserId,
                        RefreshToken = userToken.RefreshToken,
                        CreatedAt = userToken.CreatedAt,
                        ExpiredAt = userToken.ExpiredAt
                    };

                    var result = await UserTokenService.Insert(newUserToken);

                    return HttpResponse(201, "Create user token successfully", result);
                }
                catch (Exception ex)
                {
                    return ErrorResponse(ex);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDto user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var hashPass = Hash.EncryptSHA2(user.Password);

                    UserModel newUser = new UserModel()
                    {
                        Name = user.Name,
                        Username = user.Username,
                        Password = hashPass,
                    };

                    var result = await UserService.Insert(newUser);

                    return HttpResponse(201, "Create user successfully", result.AsUserNoPasswordDto());
                }
                catch (Exception ex)
                {
                    return ErrorResponse(ex);
                }
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromQuery] UpdateUserDto user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserModel mUser = await UserService.Get(id);
                   
                    mUser.Name = user.body.Name;
                    mUser.Username = user.body.Username;

                    if (!string.IsNullOrEmpty(user.body.Password))
                    {
                        var hashPass = Hash.EncryptSHA2(user.body.Password);
                        mUser.Password = hashPass;
                    }

                    var update = await UserService.Update(mUser);

                    return HttpResponse(200, "Update user successfully", mUser.AsUserNoPasswordDto());
                }
                catch (Exception ex)
                {
                    return ErrorResponse(ex);
                }
            }

            return BadRequest();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserModel mUser = await UserService.Get(id);
                    if (mUser is null)
                        return HttpResponse(404, "User not found");

                    var delete = await UserService.Delete(mUser);

                    return HttpResponse(200, "Delete user successfully", mUser.AsUserNoPasswordDto());
                }
                catch (Exception ex)
                {
                    return ErrorResponse(ex);
                }
            }
            return BadRequest();
        }

    }
}
