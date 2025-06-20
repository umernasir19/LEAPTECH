using Microsoft.Data.SqlClient;
using SEP.Model;
using System.Data;
using System.Reflection.Metadata;

namespace SEP.DAL
{
    public class Campaigns_DAL:SEP_CON_ST
    {
        SEP_Campaign objcampain;
        SEP_CampaignAPFile objapfile;

        public Campaigns_DAL() { }
         
        public Campaigns_DAL(SEP_Campaign objcampain)
        {
            this.objcampain = objcampain;
        }
        public Campaigns_DAL(SEP_CampaignAPFile objapfile)
        {
            this.objapfile = objapfile;
        }

        public SEP_Campaign CreateCampaign()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_InsertCampaign";
                cmd.Connection = con;
                con.Open();

                cmd.Parameters.AddWithValue("@BuyerID",objcampain.BuyerID);
                cmd.Parameters.AddWithValue("@CampaignName", objcampain.CampaignName);
                cmd.Parameters.AddWithValue("@Amount1", objcampain.Amount1);
                cmd.Parameters.AddWithValue("@Amount2", objcampain.Amount2);
                cmd.Parameters.AddWithValue("@Amount3", objcampain.Amount3);
                cmd.Parameters.AddWithValue("@Amount4", objcampain.Amount4);
                cmd.Parameters.AddWithValue("@Amount5", objcampain.Amount5);

                // Output parameter
                SqlParameter outParam = new SqlParameter("@CampaignID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);
               

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int StatusCode = Convert.ToInt32(reader["StatusCode"]);
                        string Message = reader["Message"].ToString();
                        objcampain.CampaignID = Convert.ToInt32(reader["CampaignID"]);
                    }
                }


                return objcampain;

            }
            catch (Exception e) { return new SEP_Campaign(); }
            finally {con.Close(); }
        }


        public SEP_CampaignAPFile SaveApFile()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_InsertCampaignAPFile";
                cmd.Connection = con;
                

                cmd.Parameters.AddWithValue("@CampaignID", objapfile.CampaignID);
                cmd.Parameters.AddWithValue("@FileName", objapfile.FileName);
                cmd.Parameters.AddWithValue("@UploadedBy", objapfile.UploadedBy);

                SqlParameter outParam = new SqlParameter("@FileID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);
               // cmd.Parameters.AddWithValue("@FileID", objapfile.UploadedBy);

                con.Open();
                this.StartTransaction();
                cmd.Transaction = this._transaction;
                cmd.ExecuteNonQuery();

                if (objapfile.detaildata != null) 
                {
                    foreach (DataRow row in objapfile.detaildata.Rows)
                    {
                        row["FileID"] =Convert.ToInt32(cmd.Parameters["@FileID"].Value.ToString());
                        //row["Master_Table_Id"] = cmd.Parameters["@ID"].Value.ToString();
                        //row["Master_Table_Name"] = "Purchase Order";
                        //row["Document_Table_Name"] = "Purchase Requisition";
                    }
                    int p_docId = Convert.ToInt32(cmd.Parameters["@FileID"].Value.ToString());
                    objapfile.detaildata.AcceptChanges();

                    SqlBulkCopy sbc = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, this._transaction);
                    objapfile.detaildata.TableName = "SEP_CampaignAPData";
                    sbc.ColumnMappings.Add("FileID", "FileID");
                    sbc.ColumnMappings.Add("SupplierName", "SupplierName");
                    sbc.ColumnMappings.Add("SupplierEmail1", "SupplierEmail1");
                    sbc.ColumnMappings.Add("ContactInfo", "ContactInfo");
                    sbc.ColumnMappings.Add("SupplierEmail2", "SupplierEmail2");
                    sbc.ColumnMappings.Add("SupplierCode", "SupplierCode");
                    sbc.ColumnMappings.Add("SupplierAddress", "SupplierAddress");
                    sbc.ColumnMappings.Add("Amount", "Amount");
                    sbc.ColumnMappings.Add("PaymentMethod", "PaymentMethod");
                    sbc.ColumnMappings.Add("PaymentTerms", "PaymentTerms");
                    sbc.ColumnMappings.Add("DPO", "DPO");
                    sbc.ColumnMappings.Add("Currency", "Currency");
                    sbc.ColumnMappings.Add("Country", "Country");
                    sbc.ColumnMappings.Add("NumberOfInvoices", "NumberOfInvoices");
                    sbc.ColumnMappings.Add("NumberOfTransactions", "NumberOfTransactions");
                    sbc.ColumnMappings.Add("NumberOfPOs", "NumberOfPOs");
                    sbc.ColumnMappings.Add("EarlyPayDiscount", "EarlyPayDiscount");
                    sbc.ColumnMappings.Add("LatePaymentPenalty", "LatePaymentPenalty");

                    sbc.DestinationTableName = "SEP_CampaignAPData"; // use your actual table name

                    sbc.WriteToServer(objapfile.detaildata);
                    this.Commit();

                }


                return objapfile;

            }
            catch (Exception ex)
            {
                this.Rollback();
                return new SEP_CampaignAPFile();

            }
            finally{
                con.Close();
            }
        }
    }
}
