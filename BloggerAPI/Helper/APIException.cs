using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloggerAPI.Helper
{
    public class UnknownException : Exception
    {
        public UnknownException() : base("Caused undefined error.") { }
        public UnknownException(string message) : base("Caused undefined error.", new Exception(message)) { }
        public UnknownException(string message, Exception innerException) { }
    }

    /// <summary>
    /// Ошибка, которая вызывается если пользователь отверг приглашение.
    /// </summary>
    public class UserRejectException : Exception
    {
        public UserRejectException() : base("User rejected the invitation.") { }
        public UserRejectException(string message) : base(message) { }
        public UserRejectException(string message, Exception innerException) { }
    }
}