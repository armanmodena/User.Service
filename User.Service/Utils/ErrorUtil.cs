using System;
using User.Service.DTO;

namespace User.Service.Utils
{

    [Serializable]
    public class ErrorUtil
    {
        public static readonly ErrorDto SecurityCodeInvalid = new ErrorDto() { code = "401", message = "Invalid sesurity code" };
        public static readonly ErrorDto InvalidModelState = new ErrorDto() { code = "10000", message = "Invalid model state" };
    }
}
