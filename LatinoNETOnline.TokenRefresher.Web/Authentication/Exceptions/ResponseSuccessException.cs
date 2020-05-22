using System;

namespace LatinoNETOnline.TokenRefresher.Web.Authentication.Exceptions
{

    [Serializable]
    public class ResponseSuccessException : Exception
    {
        public ResponseSuccessException() { }
        public ResponseSuccessException(string message) : base(message) { }
        public ResponseSuccessException(string message, Exception inner) : base(message, inner) { }
        protected ResponseSuccessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
