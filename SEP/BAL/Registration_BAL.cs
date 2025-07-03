using SEP.DAL;
using SEP.Model;

namespace SEP.BAL
{
    public class Registration_BAL
    {
        
        SEP_Account objaccnt;
        Registration_DAL objrgstrdal;

        public Registration_BAL(SEP_Account objsepaccount)
        {
            this.objaccnt = objsepaccount;
        }
        public Registration_BAL()
        {

        }

        public string CheckPhoneEmail()
        {
            objrgstrdal = new Registration_DAL(objaccnt);
            return objrgstrdal.CheckEmailPhone();
            
        }
        
        public bool VerifyOTP(OTP_Request objOTP)
        {
            objrgstrdal = new Registration_DAL();
            return objrgstrdal.VerifyOTP(objOTP);
        }

        public bool RegisterUser()
        {
            
            objrgstrdal = new Registration_DAL(objaccnt);
            
            return objrgstrdal.RegisterAccount();
        }

        public SEP_User UserLogin(string email, string password) {
            objrgstrdal = new Registration_DAL(objaccnt);
            return objrgstrdal.Login(email,  password);
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();
            return otp;
        }
    }
}
