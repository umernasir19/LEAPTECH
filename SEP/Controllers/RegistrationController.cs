using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SEP.BAL;
using SEP.Model;

namespace SEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        Registration_BAL objrgstrbal;

        [Route("Register")]
        [HttpPost]

        public IActionResult Register(SEP_Account _objaccount)
        {
            if (ModelState.IsValid) 
            {
                objrgstrbal = new Registration_BAL(_objaccount);
                string existsResult = objrgstrbal.CheckPhoneEmail();
                if (existsResult != ""){
                    return Conflict(new { message = existsResult });
                }
                objrgstrbal.RegisterUser();
                return Ok(ModelState);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
