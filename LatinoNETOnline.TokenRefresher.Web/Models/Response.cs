using System.Collections.Generic;

namespace LatinoNETOnline.TokenRefresher.Web.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class Response<T> : Response
    {
        public Response()
        {
        }

        public Response(T result)
        {
            Result = result;
            Success = true;
        }

        public T Result { get; set; }
    }

    public class ResponseEnumerable<T> : Response
    {
        public ResponseEnumerable()
        {
        }

        public ResponseEnumerable(IEnumerable<T> result)
        {
            Result = result;
            Success = true;
        }

        public IEnumerable<T> Result { get; set; }
    }
}
