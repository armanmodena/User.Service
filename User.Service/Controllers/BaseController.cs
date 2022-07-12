using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

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

        public class ResponseData
        {

            public int Status { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Message { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public object? Data { get; set; }
        }

        protected ObjectResult HttpResponse(int code = 200, string message = "", object data = null)
        {
            var response = new ResponseData
            {
                Status = code,
                Message = message,
                Data = data
            };
            return StatusCode(200, response);
        }
        protected ObjectResult HttpResponse(int code = 200, object data = null)
        {
            var response = new ResponseData
            {
                Status = code,
                Data = data
            };
            return StatusCode(200, response);

        }
    }
}
