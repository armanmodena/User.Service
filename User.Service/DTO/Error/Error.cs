using System;

namespace User.Service.DTO.Error
{

    [Serializable]
    public class Error
    {
        public string code { get; set; } = null;

        // A human-readable representation of the error.
        public string message { get; set; } = null;

    }
}
