using System;
using User.Service.DTO.Error;

namespace User.Service.Responses
{
    [Serializable]
    public class ErrorResponse
    {
        public Error Error { get; set; }
    }
}
