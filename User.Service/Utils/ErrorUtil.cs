using System;
using User.Service.DTO.Error;
using User.Service.Responses;

namespace User.Service.Utils
{

    [Serializable]
    public class ErrorUtil
    {
        public static readonly Error SecurityCodeInvalid = new Error() { code = "401", message = "Invalid sesurity code" };
        public static readonly Error InvalidModelState = new Error() { code = "10000", message = "Invalid model state" };
    }
}
