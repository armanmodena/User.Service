using System;

namespace User.Service.DTO
{

    [Serializable]
    public class ErrorDto
    {
        public string code { get; set; } = null;

        // A human-readable representation of the error.
        public string message { get; set; } = null;

    }
}
