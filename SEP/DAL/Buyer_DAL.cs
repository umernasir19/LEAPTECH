using Microsoft.Data.SqlClient;
using SEP.Model;
using System.Data;

namespace SEP.DAL
{
    public class Buyer_DAL : SEP_CON_ST
    {
        SEP_BuyerAsset _objbuyerasst;
        public Buyer_DAL()
        {

        }

        public Buyer_DAL(SEP_BuyerAsset objbuyerasset)
        {
            _objbuyerasst = objbuyerasset;
        }


        public List<SEP_BuyerAsset> GetBuyerAssets()
        {
            try
            {
                var Buyerassets = new List<SEP_BuyerAsset>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetBuyerAssets";
                cmd.Connection = con;
                con.Open();
                cmd.Parameters.AddWithValue("@BuyerID", _objbuyerasst.BuyerID);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var objbuyerasset = new SEP_BuyerAsset
                        {
                            AssetID = Convert.ToInt32(reader["AssetID"]),
                            BuyerID = Convert.ToInt32(reader["BuyerID"]),
                            FileName = reader["FileName"].ToString(),
                            FilePath = reader["FilePath"].ToString(),
                            AssetType = reader["AssetType"].ToString(),

                            UploadedAt = Convert.ToDateTime(reader["UploadedAt"].ToString())
                        };

                        Buyerassets.Add(objbuyerasset);
                    }
                }
                return Buyerassets;


            }
            catch (Exception ex)
            {
                return new List<SEP_BuyerAsset>();
            }
            finally
            {
                con.Close();
            }

        }
    }
}
