﻿using Microsoft.Data.SqlClient;

namespace SEP.DAL
{
    public class SEP_CON_ST
    {
        public readonly IConfiguration _config;
        protected SqlConnection con;
        protected SqlTransaction _transaction;
        public SEP_CON_ST()
        {
            _config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

            con = new SqlConnection(_config.GetConnectionString("SEP_DB").ToString());
        }
        public void StartTransaction()
        {
            _transaction = con.BeginTransaction();
        }
        public void Commit()
        {
            _transaction.Commit();
        }
        public void Rollback()
        {
            _transaction.Rollback();
        }
    }
}
