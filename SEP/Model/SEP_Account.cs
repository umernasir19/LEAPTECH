using System.ComponentModel.DataAnnotations;

namespace SEP.Model
{
    public class SEP_Account
    {
        public int UserID { get; set; }
        [Required(ErrorMessage ="First Name Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Registration Type Required")]
        public AccountType Type { get; set; }
        [Required(ErrorMessage = "Email Required")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Password Required")]
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
        [Required(ErrorMessage = "Email Required")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
    }

    public class OTP_Request
    {
        [Required(ErrorMessage = "UserId Required")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "OTP Required")]
        public string OTP { get; set; }
        [Required(ErrorMessage = "OTP Type Required")]
        public string OTPType { get; set; }
    }
    public enum AccountType
    {
        Buyer,
        Supplier    
}
}


