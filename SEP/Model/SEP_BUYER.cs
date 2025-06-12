using System.ComponentModel.DataAnnotations;

namespace SEP.Model
{
    public class SEP_BUYER
    {

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        [Required, MinLength(6),DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Address { get; set; }


    }
}
