using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SEP.BAL;
using SEP.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    return Conflict(new {data=_objaccount,code=409, message = existsResult });
                }
                objrgstrbal.RegisterUser();
                return Ok(new {data=_objaccount,code=200,message="Successfully Registered"});
            }
            else
            {
                return BadRequest(new SEP_Response() { Code = 401, Message = "Model Not Valid", Data = ModelState });
            }
        }


        [Route("VerifyOTP")]
        [HttpPost]
        public IActionResult VerifyOTP(OTP_Request objotp)
        {
            if (objotp .UserId>0&& objotp.OTP != "" && objotp.OTPType!="")
            {
                objrgstrbal = new Registration_BAL();
                bool flag=objrgstrbal.VerifyOTP(objotp);
                return Ok(new SEP_Response() { Code = 200, Message = flag==true?"Validation completed":"Expired or Invalid OTP", Data = objotp });

            }
            else
            {
                return BadRequest(new { data = ModelState, code = 401, message = "Not Valid object Passed" });
            }
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(LoginRequest objlogin) 
        {
            if(objlogin.EmailAddress != "" && objlogin.Password != "")
            {
                objrgstrbal = new Registration_BAL();
                var response = objrgstrbal.UserLogin(objlogin.EmailAddress, objlogin.Password);
                return Ok(new SEP_Response() { Code = 200, Message = response.Message, Data = response });
            }
            else
            {
                return BadRequest(new SEP_Response() { Code = 400, Message="Model Not Valid", Data="" });
            }
        }

    }
}
