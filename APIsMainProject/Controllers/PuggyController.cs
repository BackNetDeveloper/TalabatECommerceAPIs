using APIsMainProject.ResponseModule;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIsMainProject.Controllers
{
   
    public class PuggyController : BaseController
    {
        private readonly StoreDbContext context;

        public PuggyController(StoreDbContext context)
        {
            this.context = context;
        }
        [HttpGet("GetMessage")]
        public ActionResult<string> GetMessage()
        {
            return "Testing Message";
        }
        [HttpGet("GetBadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("GetNotFoundt")]
        public ActionResult GetNotFoundt()
        {
            var Data = context.Products.Find(1000);
            if (Data == null)
                return BadRequest(new ApiResponse(404));
            else
                return Ok();
        }
    }
}
