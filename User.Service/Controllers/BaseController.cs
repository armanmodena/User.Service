using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using User.Service.DTO;

namespace User.Service.Controllers
{
    public class BaseController : Controller
    {
        protected ObjectResult ErrorResponse(Exception ex, int code = 500)
        {
            var error = new
            {
                Status = code,
                Message = ex.Message,
                InnerException = ex.InnerException?.Message,
                Data = ex.Data,
                Source = ex.Source
            };
            return StatusCode(code, error);
        }

        protected ObjectResult HttpResponse(int code = 200, string message = "", object data = null)
        {
            var response = new ResponseDataDto
            {
                Status = code,
                Message = message,
                Data = data
            };
            return StatusCode(code, response);
        }
        protected ObjectResult HttpResponse(int code = 200, object data = null)
        {
            var response = new ResponseDataDto
            {
                Status = code,
                Data = data
            };
            return StatusCode(code, response);

        }
    }
}
