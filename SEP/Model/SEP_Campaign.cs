namespace SEP.Model
{
    public class SEP_Campaign
    {
        public int CampaignID { get; set; }
        public int BuyerID { get; set; }
        public string CampaignName { get; set; }

        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
        public decimal Amount3 { get; set; }
        public decimal Amount4 { get; set; }
        public decimal Amount5 { get; set; }

        public IFormFile APFile { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class SEP_CampaignAPFile
    {
        public int FileID { get; set; }
        public int CampaignID { get; set; }
        public string FileName { get; set; }
        public DateTime UploadedAt { get; set; }
        public int? UploadedBy { get; set; } // optional
        public string Remarks { get; set; }
    }
    public class SEP_CampaignAPData
    {
        public int RowID { get; set; }
        public int FileID { get; set; }

        public string SupplierName { get; set; }
        public string SupplierEmail1 { get; set; }
        public string ContactInfo { get; set; }
        public string SupplierEmail2 { get; set; }
        public string SupplierCode { get; set; } // corresponds to Supplier ID in Excel
        public string SupplierAddress { get; set; }

        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentTerms { get; set; }
        public int? DPO { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }

        public int? NumberOfInvoices { get; set; }
        public int? NumberOfTransactions { get; set; }
        public int? NumberOfPOs { get; set; }
        public bool? EarlyPayDiscount { get; set; }
        public bool? LatePaymentPenalty { get; set; }
    }

}
