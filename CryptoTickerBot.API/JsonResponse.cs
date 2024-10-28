namespace CryptoTickerBot.API
{
    public class JsonResponse
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public JsonResponse(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}