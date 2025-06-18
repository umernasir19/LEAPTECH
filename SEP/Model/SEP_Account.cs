namespace SEP.Model
{
    public class SEP_Account
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public AccountType Type { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public int CountryCode { get; set; }

        public string Designation { get; set; }

        public string  Department { get; set; }
    }

    public class Login_User:SEP_Account
    {
        public int Status { get; set; }
        public string Message { get; set; }

    }
    public class SEP_User:Login_User
    {
        //public int UserId { get; set; }

    }
    public class LoginRequest
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }

    public class OTP_Request
    {
        public int UserId { get; set; }

        public string OTP { get; set; }
        public string OTPType { get; set; }
    }
    public enum AccountType
    {
        Buyer,
        Supplier    
}
}


