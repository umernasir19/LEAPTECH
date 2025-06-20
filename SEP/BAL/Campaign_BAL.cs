using SEP.DAL;
using SEP.Model;

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

    }
}
