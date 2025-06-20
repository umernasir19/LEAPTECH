using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEP.BAL;
using SEP.Model;
using SEP.Utility;
using System.Data;
using System.Threading.Tasks;

namespace SEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        Campaign_BAL objcampaignbal;

        [HttpPost]
        [Route("CreateCampaign")]
        public IActionResult Createcampaign(SEP_Campaign objcam)
        {
            if (ModelState.IsValid)
            {

                objcampaignbal = new Campaign_BAL(objcam);
                var response = objcampaignbal.CreateCampain();
                if (response.CampaignID > 0)
                {
                    //calling method to save files
                    return Ok(new SEP_Response() { Code = 200, Message = "", Data = response });
                }
                else
                {
                    return BadRequest(new SEP_Response() { Code = 400, Message = "Failed", Data = response });
                }
            }
            else
            {
                return BadRequest(new SEP_Response() { Code = 400, Message = "ModelState", Data = ModelState });
            }


        }

        [HttpPost]
        [Route("UploadAPFile")]
        public async Task<IActionResult> UploadApFile(SEP_CampaignAPFile objcampaignAPFile)
        {
            if (ModelState.IsValid)
            {
                if (objcampaignAPFile.APFile.ContentType == "text/csv")
                {
                    //objcampaignAPFile.FileName = objcampaignAPFile.APFile.FileName;
                    string APfolderPAth = Path.Combine(Directory.GetCurrentDirectory(), "APFiles");
                    //read CSV and return to list
                    List<SEP_CampaignAPData> aplist =await Utilities.SaveAndParseCsvAsync(objcampaignAPFile, APfolderPAth);
                    //list convert to datatable for DB sending
                    DataTable apfiledata = Utilities.ToDataTable<SEP_CampaignAPData>(aplist);
                    objcampaignAPFile.detaildata = apfiledata;
                    objcampaignbal = new Campaign_BAL(objcampaignAPFile);
                    var response = objcampaignbal.SaveApfileData();
                    return Ok(new SEP_Response() { Data = response, Code = 200, Message = "File Saved" });
                }

                return Ok(new SEP_Response() { Data = ModelState, Code = 400, Message = "File Format not supported" });

            }
            else
            {
                return BadRequest(new SEP_Response() { Data = ModelState, Code = 400, Message = "Model Not Valid" });
            }
        }



    }
}
