using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTickerBot.API
{
    public class HttpResponseException : Exception
    {
        public int Status { get; set; } = 500;

        public object Value { get; set; }


        public HttpResponseException(Exception ex) : base(ex.Message)
        {

        }
        public HttpResponseException(string message) : base(message)
        {
        }


    }
}