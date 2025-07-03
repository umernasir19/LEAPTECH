using SEP.DAL;
using SEP.Model;
using SEP.Utility;
using System.Data;

namespace SEP.BAL
{
    public class Campaign_BAL
    {
        Campaigns_DAL objcampaigndal;
        SEP_Campaign objcampaignmodel;
        SEP_CampaignAPFile _objapfile;

        public Campaign_BAL()
        { }

        public Campaign_BAL(SEP_Campaign objcampaign)
        {
            this.objcampaignmodel = objcampaign;
        }

        public Campaign_BAL(SEP_CampaignAPFile objapfile)
        {
            this._objapfile = objapfile;
        }

        public SEP_Campaign CreateCampain()
        {
            objcampaigndal = new Campaigns_DAL(objcampaignmodel);
            return objcampaigndal.CreateCampaign();
        }

        public SEP_CampaignAPFile SaveApfileData()
        {
            objcampaigndal = new Campaigns_DAL(_objapfile);
            return objcampaigndal.SaveApFile();
        }

        public List<SEP_Campaign> GetCampaignsByBuyerId()
        {
            objcampaigndal = new Campaigns_DAL(objcampaignmodel);
            return objcampaigndal.GetCampaignsByBuyerId();

        }

        public List<SEP_Campaign_Results> GetCampainApFiles()
        {
            objcampaigndal = new Campaigns_DAL(_objapfile);
            return objcampaigndal.GetCampainApFiles();
        }


        public List<SEP_BuyerAsset> EndorsementLetterPrepareandSave(SEP_Upload_logo objlogo,string baseurl)
        {
           var objbuyerassetdata= Utilities.ProcessEndoresement(objlogo, baseurl);
            DataTable dt = Utilities.ToDataTable<SEP_BuyerAsset>(objbuyerassetdata);
            objcampaigndal = new Campaigns_DAL();
            if(objcampaigndal.SaveEndorsment(dt))
            {
                return objbuyerassetdata;
            }
            else
            {
                return new List<SEP_BuyerAsset>();
            }

        }


        public bool UpdateBuyerActions(SEP_BuyerAction objbuyeracton)
        {
            objcampaigndal = new Campaigns_DAL();
            return objcampaigndal.ChangeCampaignStatus(objbuyeracton);
        }
    }
}
