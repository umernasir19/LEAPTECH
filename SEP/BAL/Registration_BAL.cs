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

        public string CheckPhoneEmail()
        {
            objrgstrdal = new Registration_DAL(objaccnt);
            return objrgstrdal.CheckEmailPhone();
            
        }
        

        public bool RegisterUser()
        {
            objrgstrdal = new Registration_DAL(objaccnt);

            return objrgstrdal.RegisterAccount();
        }

    }
}
