using SEP.Model;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Net.Mail;
using System.Numerics;
using Azure;

namespace SEP.DAL
{
    public class Registration_DAL : SEP_CON_ST
    {
        SEP_Account objrgstr;

        public Registration_DAL(SEP_Account _objrgstr)
        {
            this.objrgstr = _objrgstr;
        }

        public Registration_DAL()
        {

        }

        public string CheckEmailPhone()
        {
            string msg = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_CheckIfUserExists";
            cmd.Connection = con;
            try
            {
                cmd.Parameters.AddWithValue("@EmailAddr", objrgstr.EmailAddress);
                cmd.Parameters.AddWithValue("@PhoneNum", objrgstr.Phone);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    string existingEmail = reader["Email"].ToString();
                    string existingPhone = reader["Phone"].ToString();

                    if (existingEmail == objrgstr.EmailAddress)
                        msg = "Email already exists.";
                    else if (existingPhone == objrgstr.Phone)
                        msg = "Phone number already exists.";
                }
            }
            catch (Exception ex)
            {
            }
            return msg;
        }

        public bool RegisterAccount()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_registeruser_full";
                cmd.Connection = con;
                con.Open();

                cmd.Parameters.Add(new SqlParameter("@Organization", SqlDbType.NVarChar, 100, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objrgstr.Organization));

                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objrgstr.EmailAddress));
                cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objrgstr.Phone));
                cmd.Parameters.Add(new SqlParameter("@UserType", SqlDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, objrgstr.Type));
                cmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, objrgstr.Password));
                cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, objrgstr.FirstName));
                cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, objrgstr.LastName));

                cmd.Parameters.Add(new SqlParameter("@Country", SqlDbType.Int, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, objrgstr.CountryCode));
                cmd.Parameters.Add(new SqlParameter("@Designation", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, objrgstr.Designation));
                cmd.Parameters.Add(new SqlParameter("@Department", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, objrgstr.Department));

                cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar, 30, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Proposed, objrgstr.CountryCode));

                cmd.ExecuteNonQuery();
                object value = cmd.Parameters["@UserId"].Value;

                int userId = (value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
                    ? 0
                    : Convert.ToInt32(value);

                objrgstr.UserID = userId;
                return true;
            }
            catch (Exception ex)
            {


                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public bool VerifyOTP(OTP_Request objOTP)
        {
            try
            {
                bool verifyOTP = false;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_verify_otp";
                cmd.Connection = con;
                con.Open();
                cmd.Parameters.AddWithValue("@UserId", objOTP.UserId);
                cmd.Parameters.AddWithValue("@OTP", objOTP.OTP);
                cmd.Parameters.AddWithValue("@OTPType", objOTP.OTPType);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        int Status = reader.GetInt32(reader.GetOrdinal("StatusCode"));
                        string Message = reader["Message"].ToString();

                        // If  successful, map additional fields
                        if (Status == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

               
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { con.Close(); }
        }

        public SEP_User Login(string username, string password) {

            try
            {
                var response = new SEP_User();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.CommandText = "sp_login_user";
                cmd.Connection = con;
                con.Open();
                
                cmd.Parameters.AddWithValue ("@Email", username);
                cmd.Parameters.AddWithValue ("@Password", password);
                using (SqlDataReader reader = cmd.ExecuteReader()) 
                {

                    if (reader.Read())
                    {
                        response.Status = reader.GetInt32(reader.GetOrdinal("StatusCode"));
                        response.Message = reader["Message"].ToString();

                        // If login successful, map additional fields
                        if (response.Status == 1)
                        {
                            response.UserID = reader.GetInt32(reader.GetOrdinal("UserId"));
                            response.EmailAddress = reader["Email"].ToString();
                            response.Phone = reader["Phone"].ToString();
                            response.Type = (AccountType)Enum.Parse(typeof(AccountType), reader["UserType"].ToString());
                            response.FirstName = reader["FirstName"]?.ToString();
                            response.LastName = reader["LastName"]?.ToString();
                            response.CountryCode = reader["CountryCode"] != DBNull.Value ? Convert.ToInt32(reader["CountryCode"]) : 0;
                            response.Designation = reader["Designation"]?.ToString();
                            response.Department = reader["Department"]?.ToString();
                        }
                        if (response.Status == -1)
                        {
                            response.Message = response.Message = reader["Message"].ToString();
                        }
                        if (response.Status == -2) 
                        {
                            response.Message = response.Message = reader["Message"].ToString();
                        }
                    }
                }


                return  response;

            }
            catch (Exception ex) {
                return new SEP_User();
            }
            finally { con.Close(); }
        
        }
    }
}