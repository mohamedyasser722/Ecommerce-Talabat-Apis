namespace Talabat.APIs.Errors
{
    public class APiValidationErrorResponse : ApiErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public APiValidationErrorResponse() : base(400)
        {
            Errors = new List<string>();
        }
    }
}
