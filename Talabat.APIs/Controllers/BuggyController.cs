using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{

    public class BuggyController : BaseApiController
    {
        private readonly StoreContext context;

        public BuggyController(StoreContext context)
        {
            this.context = context;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest() => NotFound(new ApiErrorResponse(404));

        [HttpGet("badrequest")]
        public ActionResult GetBadRequestError() => BadRequest(new ApiErrorResponse(400));

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequestError(int id) => Ok();

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var Product = context.Products.Find(100);
            var ProductToReturn = Product.ToString();//will throw Exception
            return Ok(ProductToReturn);
        }

    }
}
