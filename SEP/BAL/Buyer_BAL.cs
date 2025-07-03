using SEP.DAL;
using SEP.Model;

namespace SEP.BAL
{
    public class Buyer_BAL
    {
        SEP_BuyerAsset _objbuyerasset;
        Buyer_DAL _objbuyerdal;

        public Buyer_BAL()
        {

        }

        public Buyer_BAL(SEP_BuyerAsset objbuyerasset)
        {
            _objbuyerasset = objbuyerasset;
        }

        public List<SEP_BuyerAsset> GetBuyerAssets()
        {
            _objbuyerdal=new Buyer_DAL(_objbuyerasset);
            return _objbuyerdal.GetBuyerAssets();
        }

    }
}
