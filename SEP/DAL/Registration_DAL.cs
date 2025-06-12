using SEP.Model;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Numerics;

namespace SEP.DAL
{
    public class Registration_DAL : SEP_CON_ST
    {
        SEP_Account objrgstr;

        public Registration_DAL(SEP_Account _objrgstr)
        {
            this.objrgstr = _objrgstr;
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
            catch (Exception ex) { 
            }
            return msg;
        }

        public bool RegisterAccount()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_registeruser";
                cmd.Connection = con;
                con.Open();


                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objrgstr.EmailAddress));
                cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objrgstr.Phone));
                cmd.Parameters.Add(new SqlParameter("@UserType", SqlDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, objrgstr.Type));
                cmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, objrgstr.FirstName));
                cmd.Parameters.Add(new SqlParameter("@iid", SqlDbType.VarChar, 30, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Proposed, objrgstr.FirstName));

                cmd.ExecuteNonQuery();
                object value = cmd.Parameters["@iid"].Value;

                int userId = (value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
                    ? 0
                    : Convert.ToInt32(value);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}