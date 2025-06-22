using System.ComponentModel.DataAnnotations;

namespace SEP.Model
{
    public class SEP_BUYER
    {

        public int BuyerId { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }

        public string  FirstName { get; set; }

        public string  LastName { get; set; }

        public string Organization { get; set; }

        public int CountryCode { get; set; }

        public string Designation { get; set; }

        public string Department { get; set; }





    }
}
