using Microsoft.Data.SqlClient;
using SEP.Model;
using System.Data;
using System.Reflection.Metadata;

namespace SEP.DAL
{
    public class Campaigns_DAL : SEP_CON_ST
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

                cmd.Parameters.AddWithValue("@BuyerID", objcampain.BuyerID);
                cmd.Parameters.AddWithValue("@CampaignName", objcampain.CampaignName);
                cmd.Parameters.AddWithValue("@Amount1", objcampain.Amount1);
                cmd.Parameters.AddWithValue("@Amount2", objcampain.Amount2);
                cmd.Parameters.AddWithValue("@Amount3", objcampain.Amount3);
                cmd.Parameters.AddWithValue("@Amount4", objcampain.Amount4);
                cmd.Parameters.AddWithValue("@Amount5", objcampain.Amount5);
                cmd.Parameters.AddWithValue("@FileId", objcampain.FileId);


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
            finally { con.Close(); }
        }


        public SEP_CampaignAPFile SaveApFile()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_InsertCampaignAPFile";
                cmd.Connection = con;


                //cmd.Parameters.AddWithValue("@CampaignID", objapfile.CampaignID);
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

                if (Convert.ToInt32(cmd.Parameters["@FileID"].Value.ToString()) > 0)
                {
                    objapfile.FileID = Convert.ToInt32(cmd.Parameters["@FileID"].Value.ToString());
                }


                if (objapfile.detaildata != null)
                {
                    foreach (DataRow row in objapfile.detaildata.Rows)
                    {
                        row["FileID"] = Convert.ToInt32(cmd.Parameters["@FileID"].Value.ToString());
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
            finally
            {
                con.Close();
            }
        }

        public List<SEP_Campaign> GetCampaignsByBuyerId()
        {
            try
            {
                var campaigns = new List<SEP_Campaign>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetCampaignsByBuyer";
                cmd.Connection = con;
                con.Open();
                cmd.Parameters.AddWithValue("@BuyerID", objcampain.BuyerID);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var campaign = new SEP_Campaign
                        {
                            CampaignID = Convert.ToInt32(reader["CampaignID"]),
                            BuyerID = Convert.ToInt32(reader["BuyerID"]),
                            CampaignName = reader["CampaignName"].ToString(),
                            Amount1 = reader.GetDecimal(reader.GetOrdinal("Amount1")),
                            Amount2 = reader.GetDecimal(reader.GetOrdinal("Amount2")),
                            Amount3 = reader.GetDecimal(reader.GetOrdinal("Amount3")),
                            Amount4 = reader.GetDecimal(reader.GetOrdinal("Amount4")),
                            Amount5 = reader.GetDecimal(reader.GetOrdinal("Amount5")),
                            FileId = reader["FileID"] != DBNull.Value ? Convert.ToInt32(reader["FileID"]) : 0,
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                        };

                        campaigns.Add(campaign);
                    }
                }
                return campaigns;

            }
            catch (Exception ex)
            {
                return new List<SEP_Campaign>();
            }
            finally { con.Close(); }

        }


        public List<SEP_Campaign_Results> GetCampainApFiles()
        {
            try
            {
                var listapfiledata=new List<SEP_Campaign_Results>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetCAPFileData";
                cmd.Connection = con;

                con.Open();

                cmd.Parameters.AddWithValue("@fileId", objapfile.FileID);

                using (SqlDataReader reader = cmd.ExecuteReader()) 
                {
                    int i = 0;
                    while (reader.Read()) 
                    {
                        i++;
                        var apfiledata = new SEP_Campaign_Results();

                        apfiledata.RowID = i;
                        apfiledata.FileID = reader.GetInt32(reader.GetOrdinal("FileID"));
                        apfiledata.SupplierName = reader["SupplierName"]?.ToString();
                        apfiledata.SupplierEmail1 = reader["SupplierEmail1"]?.ToString();
                        apfiledata.ContactInfo = reader["ContactInfo"]?.ToString();
                        apfiledata.SupplierEmail2 = reader["SupplierEmail2"]?.ToString();
                        apfiledata.SupplierCode = reader["SupplierCode"]?.ToString();
                        apfiledata.SupplierAddress = reader["SupplierAddress"]?.ToString();
                        apfiledata.Amount = reader.GetDecimal(reader.GetOrdinal("Amount"));
                        apfiledata.PaymentMethod = reader["PaymentMethod"]?.ToString();
                        apfiledata.PaymentTerms = reader["PaymentTerms"]?.ToString();
                        apfiledata.DPO = reader["DPO"] != DBNull.Value ? Convert.ToInt32(reader["DPO"]) : (int?)null;
                        apfiledata.Currency = reader["Currency"]?.ToString();
                        apfiledata.Country = reader["Country"]?.ToString();
                        apfiledata.NumberOfInvoices = reader["NumberOfInvoices"] != DBNull.Value ? Convert.ToInt32(reader["NumberOfInvoices"]) : (int?)null;
                        apfiledata.NumberOfTransactions = reader["NumberOfTransactions"] != DBNull.Value ? Convert.ToInt32(reader["NumberOfTransactions"]) : (int?)null;
                        apfiledata.NumberOfPOs = reader["NumberOfPOs"] != DBNull.Value ? Convert.ToInt32(reader["NumberOfPOs"]) : (int?)null;
                        apfiledata.EarlyPayDiscount = reader["EarlyPayDiscount"] != DBNull.Value ? Convert.ToBoolean(reader["EarlyPayDiscount"]) : (bool?)null;
                        apfiledata.LatePaymentPenalty = reader["LatePaymentPenalty"] != DBNull.Value ? Convert.ToBoolean(reader["LatePaymentPenalty"]) : (bool?)null;
                        apfiledata.NewPaymentTerms = reader.GetInt32(reader.GetOrdinal("NewPayterm"));




                        listapfiledata.Add(apfiledata);
                    }

                }
                return listapfiledata;


            }
            catch(Exception ex)
            {
                return new List<SEP_Campaign_Results>();
            }
            finally
            {
                con.Close();
            }
        }
    }
}
