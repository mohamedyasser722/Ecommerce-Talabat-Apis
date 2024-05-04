namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse : ApiErrorResponse
    {
        public string? Details { get; set; }

        public ApiExceptionResponse(int StatusCode, string? message = null, string? Details = null) : base(StatusCode, message)
        {
            this.Details = Details;
        }

    }
}
