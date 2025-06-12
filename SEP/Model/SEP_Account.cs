namespace SEP.Model
{
    public class SEP_Account
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public AccountType Type { get; set; }

        public string EmailAddress { get; set; }

        public string Phone { get; set; }
    }


    public enum AccountType
    {
        Buyer,
        Supplier    
}
}


