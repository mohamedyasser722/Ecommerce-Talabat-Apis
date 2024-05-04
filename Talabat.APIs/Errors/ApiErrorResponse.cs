namespace Talabat.APIs.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiErrorResponse(int StatusCode, string? message = null)
        {
            this.StatusCode = StatusCode;
            Message = message ?? GetDefaultMessageForStatusCode(StatusCode)?? $"Status Code {StatusCode} Not Implemented in Our Default Messages";
        }

        private string? GetDefaultMessageForStatusCode(int StatusCode)
        {
            return StatusCode switch
            {
                400 => "A bad Request, You Have Made",
                401 => "You Are Not Authorized",
                404 => "Resources Not Found",
                500 => "There is Server Error",
                _ => null
            };
        }
    }
}
