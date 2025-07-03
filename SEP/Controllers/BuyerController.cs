using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEP.BAL;
using SEP.Model;

namespace SEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        Buyer_BAL _objbuyerbal;
        [HttpGet]
        [Route("GetBuyerAssets")]
        public IActionResult GetBuyerAssets(int buyerid)
        {
            SEP_BuyerAsset objbuyerasset = new SEP_BuyerAsset();
            objbuyerasset.BuyerID = buyerid;
            _objbuyerbal = new Buyer_BAL(objbuyerasset);
            var flag = _objbuyerbal.GetBuyerAssets();
            if (flag.Count > 0)
            {
                return Ok(new SEP_Response() { Data = flag, Code = 200, Message = "Records Returned" });
            }
            else
            {
                return NotFound(new SEP_Response() { Code=404,Message="No Data Found",Data=null});
            }
        }
    }
}
