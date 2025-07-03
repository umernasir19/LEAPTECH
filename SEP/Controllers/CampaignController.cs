using Azure;
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


        [HttpGet]
        [Route("GetCampaignByBuyer")]
        public IActionResult GetCampaignByBuyer(int BuyerId)
        {
            if (BuyerId > 0)
            {
                SEP_Campaign buyer = new SEP_Campaign();
                buyer.BuyerID = BuyerId;
                objcampaignbal = new Campaign_BAL(buyer);
                var response = objcampaignbal.GetCampaignsByBuyerId();
                if (response.Count > 0)
                {
                    return Ok(new SEP_Response() { Code = 200, Message = "Records Returned", Data = response });
                }
                else
                {
                    return NotFound(new SEP_Response() { Code = 404, Message = "No Record Found", Data = "" });

                }

            }
            else
            {
                return BadRequest(new SEP_Response() { Code = 400, Message = "Not Valid Request", Data = "" });
            }

        }


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



        [HttpGet]
        [Route("GetAPDataforDashboard")]
        public IActionResult GetAPDataforDashboard(int FileId)
        {
            if (FileId > 0)
            {
                SEP_CampaignAPFile buyer = new SEP_CampaignAPFile();
                buyer.FileID = FileId;
                objcampaignbal = new Campaign_BAL(buyer);
                var response = objcampaignbal.GetCampainApFiles();
                if (response.Count > 0)
                {
                    return Ok(new SEP_Response() { Code = 200, Message = "Records Returned", Data = response });
                }
                else
                {
                    return NotFound(new SEP_Response() { Code = 404, Message = "No Record Found", Data = "" });

                }

            }
            else
            {
                return BadRequest(new SEP_Response() { Code = 400, Message = "Not Valid Request", Data = "" });
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
                    List<SEP_CampaignAPData> aplist = await Utilities.SaveAndParseCsvAsync(objcampaignAPFile, APfolderPAth);
                    //list convert to datatable for DB sending
                    DataTable apfiledata = Utilities.ToDataTable<SEP_CampaignAPData>(aplist);
                    objcampaignAPFile.detaildata = apfiledata;
                    objcampaignbal = new Campaign_BAL(objcampaignAPFile);
                    var response = objcampaignbal.SaveApfileData();
                    if (response.FileID > 0)
                    {
                        return Ok(new SEP_Response() { Data = response, Code = 200, Message = "File Saved" });
                    }
                    else
                    {
                        return BadRequest(new SEP_Response() { Data = "", Code = 400, Message = "File Not Saved" });
                    }

                }

                return BadRequest(new SEP_Response() { Data = ModelState, Code = 400, Message = "File Format not supported" });

            }
            else
            {
                return BadRequest(new SEP_Response() { Data = ModelState, Code = 400, Message = "Model Not Valid" });
            }
        }


        [HttpPost]
        [Route("UploadPicturesforLetter")]
        public IActionResult UploadPicturesforLetter(SEP_Upload_logo objlogo)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            // Utilities.SendEmail(baseUrl);
            objcampaignbal = new Campaign_BAL();
            var flag = objcampaignbal.EndorsementLetterPrepareandSave(objlogo, baseUrl);

            if (flag.Count > 0)
            {
                return Ok(new SEP_Response() { Data = flag, Code = 200, Message = "File Saved" });

            }
            else
            {
                return BadRequest(new SEP_Response() { Data = "", Code = 400, Message = "File Not Saved" });

            }

        }

        [HttpPost]
        [Route("PerformBuyerAction")]
        public IActionResult PerformBuyerAction(SEP_BuyerAction objbuyeraction)
        {
            objcampaignbal = new Campaign_BAL();
            var flag = objcampaignbal.UpdateBuyerActions(objbuyeraction);
            if (flag)
            {
                return Ok(new SEP_Response()
                {
                    Code = 200,
                    Data = "",
                    Message = "Updated Successfully"
                });

            }
            else
            {
                return BadRequest(new SEP_Response() { Message="Error",Code=400,Data=null});
            }
        }
    }
}
