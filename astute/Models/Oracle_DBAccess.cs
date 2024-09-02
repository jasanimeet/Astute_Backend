using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Models
{
    public class Oracle_DBAccess : DbContext
    {
        private readonly IConfiguration _configuration;

        public Oracle_DBAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        OracleConnection connection;

        public string GetConnectionString()
        {
            string connString = _configuration["ConnectionStrings:Oraweb"].ToString();
            return connString;
        }

        public async Task<DataTable> CallSP(string SP, List<OracleParameter> paramList)
        {
            using OracleConnection connection = new OracleConnection();
            using OracleCommand cmd = new OracleCommand();
            using OracleDataAdapter da = new OracleDataAdapter();

            connection.ConnectionString = GetConnectionString();

            await connection.OpenAsync();

            cmd.Connection = connection;
            cmd.CommandText = SP;
            cmd.CommandType = CommandType.StoredProcedure;

            if (paramList != null)
            {
                foreach (var parameter in paramList)
                {
                    OracleParameter param = new OracleParameter(parameter.ParameterName, parameter.Value);
                    param.Direction = parameter.Direction;
                    param.OracleDbType = parameter.OracleDbType;
                    param.Size = parameter.Size;
                    cmd.Parameters.Add(param);
                }
            }

            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }
        public async Task<DataTable> CallSP_Timeout(string SP, List<OracleParameter> paramList)
        {
            using OracleConnection connection = new OracleConnection();
            using OracleCommand cmd = new OracleCommand();
            using OracleDataAdapter da = new OracleDataAdapter();

            connection.ConnectionString = GetConnectionString();

            await connection.OpenAsync();

            cmd.Connection = connection;
            cmd.CommandText = SP;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;

            if (paramList != null)
            {
                foreach (var parameter in paramList)
                {
                    OracleParameter param = new OracleParameter(parameter.ParameterName, parameter.Value);
                    param.Direction = parameter.Direction;
                    param.OracleDbType = parameter.OracleDbType;
                    param.Size = parameter.Size;
                    cmd.Parameters.Add(param);
                }
            }

            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }

        #region IDisposable Members
        public void Dispose()
        {
            if (connection != null)
                connection = null;
        }
        #endregion
    }
}
