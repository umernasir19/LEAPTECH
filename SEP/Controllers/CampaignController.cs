using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEP.Model;

namespace SEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        [HttpPost]
        [Route("CreateCampaign")]
        public  IActionResult Createcampaign(SEP_Campaign objcam)
        {

            return Ok(new SEP_Response() { Code = 200,Message="",Data="" });
        }
    }
}
