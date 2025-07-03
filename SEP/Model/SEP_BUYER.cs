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

    public class SEP_Upload_logo
    {
        public int BuyerId { get; set; }

        public int CampaignID { get; set; }

        public IFormFile Logo { get; set; }

        public IFormFile Signature { get; set; }



    }

    public class SEP_BuyerAsset
    {
        public int AssetID { get; set; }

        //[Required(ErrorMessage = "Buyer ID is required.")]
        public int BuyerID { get; set; }

        // [Required(ErrorMessage = "Asset type is required.")]
        // [RegularExpression("Logo|Signature", ErrorMessage = "AssetType must be either 'Logo' or 'Signature'.")]
        public string AssetType { get; set; }  // "Logo" or "Signature"

        //  [Required]
        public string FileName { get; set; }

        //[Required]
        public string FilePath { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }



}
