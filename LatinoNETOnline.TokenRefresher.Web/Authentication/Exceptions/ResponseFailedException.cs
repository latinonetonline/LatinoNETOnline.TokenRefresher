using System;

using LatinoNETOnline.TokenRefresher.Web.Models;

namespace LatinoNETOnline.TokenRefresher.Web.Authentication.Exceptions
{

    [Serializable]
    public class ResponseFailedException : Exception
    {
        public ResponseFailedException() { }
        public ResponseFailedException(Response response) : base(response.Message) { }
        public ResponseFailedException(string message) : base(message) { }
        public ResponseFailedException(string message, Exception inner) : base(message, inner) { }
        protected ResponseFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
