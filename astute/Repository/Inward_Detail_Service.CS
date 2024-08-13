using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;

namespace astute.Repository
{
    public class Inward_Detail_Service : IInward_Detail_Service
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public Inward_Detail_Service(AstuteDbContext dbContext,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        #endregion

        #region Methods

        #region Import Master
        public async Task<List<Dictionary<string, object>>> Get_Import_Master()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Import_Master_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> Insert_Import_Master(Import_Master import_Master, int user_Id)
        {
            var importid = new SqlParameter("@Import_Id", import_Master.Import_Id);
            var formatname = new SqlParameter("@Format_Name", import_Master.Format_Name);
            var type = new SqlParameter("@Type", import_Master.Type);
            var userId = new SqlParameter("@UserId", user_Id);
            var isReferencedParameter = new SqlParameter("@ID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Import_Master_Insert_Update @Import_Id, @Format_Name, @Type, @UserId, @ID OUT",
            importid, formatname, type, userId, isReferencedParameter));

            var Id = (int)isReferencedParameter.Value;

            return Id;
        }

        public async Task<int> Update_Import_Master(Import_Master import_Master)
        {
            var importid = new SqlParameter("@Import_Id", import_Master.Import_Id);
            var formatname = new SqlParameter("@Format_Name", import_Master.Format_Name);
            var type = new SqlParameter("@Type", import_Master.Type);
            var recordType = new SqlParameter("@RecordType", "Update");
            var isReferencedParameter = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Import_Master_Insert_Update @Import_Id, @Format_Name, @Type, @RecordType, @IsExist OUT",
            importid, formatname, type, recordType, isReferencedParameter));

            var isExist = (bool)isReferencedParameter.Value;
            if (isExist)
                return 2;

            return result;
        }

        public async Task<int> Delete_Import_Master(int id)
        {
            var isReferencedParameter = new SqlParameter("@IsReferenced", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Import_Master_Delete @Import_Id, @IsReferenced OUT",
                                        new SqlParameter("@Import_Id", id),
                                        isReferencedParameter);

            var isReferenced = (bool)isReferencedParameter.Value;
            if (isReferenced)
                return 2;

            return result;
        }
        #endregion

        #region Import Detail
        public async Task<List<Dictionary<string, object>>> Get_Import_Detail()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Import_Detail_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> Insert_Import_Detail(Import_Detail import_Detail)
        {
            var importdetid = new SqlParameter("@Import_Det_Id", import_Detail.Import_Det_Id);
            var importid = new SqlParameter("@Import_Id", import_Detail.Import_Id);
            var columnname = new SqlParameter("@Column_Name", import_Detail.Column_Name);
            var excelcolumnno = new SqlParameter("@Excel_Column_No", import_Detail.Excel_Column_No);
            var required = new SqlParameter("@Required", import_Detail.Required);
            var recordType = new SqlParameter("@RecordType", "Insert");
            var isReferencedParameter = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Import_Detail_Insert_Update @Import_Det_Id, @Import_Id, @Column_Name, @Excel_Column_No, @Required, @RecordType, @IsExist OUT",
            importdetid, importid, columnname, excelcolumnno, required, recordType, isReferencedParameter));

            var isExist = (bool)isReferencedParameter.Value;
            if (isExist)
                return 2;

            return result;
        }

        public async Task<int> Update_Import_Detail(Import_Detail import_Detail)
        {
            var importdetid = new SqlParameter("@Import_Det_Id", import_Detail.Import_Det_Id);
            var importid = new SqlParameter("@Import_Id", import_Detail.Import_Id);
            var columnname = new SqlParameter("@Column_Name", import_Detail.Column_Name);
            var excelcolumnno = new SqlParameter("@Excel_Column_No", import_Detail.Excel_Column_No);
            var required = new SqlParameter("@Required", import_Detail.Required);
            var recordType = new SqlParameter("@RecordType", "Update");
            var isReferencedParameter = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Import_Detail_Insert_Update @Import_Det_Id, @Import_Id, @Column_Name, @Excel_Column_No, @Required, @RecordType, @IsExist OUT",
            importdetid, importid, columnname, excelcolumnno, required, recordType, isReferencedParameter));

            var isExist = (bool)isReferencedParameter.Value;
            if (isExist)
                return 2;

            return result;
        }

        public async Task<int> Delete_Import_Detail(int id)
        {
            var isReferencedParameter = new SqlParameter("@IsReferenced", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Import_Detail_Delete @Import_Det_Id, @IsReferenced OUT",
                                        new SqlParameter("@Import_Det_Id", id),
                                        isReferencedParameter);

            var isReferenced = (bool)isReferencedParameter.Value;
            if (isReferenced)
                return 2;

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Import_Excel()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Import_Excel_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Import_Master_Detail(int import_Id)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"]))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("Import_Master_Detail_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Import_Id", import_Id > 0 ? import_Id : (object)DBNull.Value));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }

            return result;
        }

        public async Task<int> Insert_Update_Import_Excel(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Import_Detail", SqlDbType.Structured)
            {
                TypeName = "dbo.Import_Detail_Data_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Import_Excel_Insert_Update @Import_Detail", parameter));

            return result;
        }

        public async Task<int> Delete_Import_Excel(int id)
        {
            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Import_Master_Detail_Delete @Import_Id",
                                        new SqlParameter("@Import_Id", id));

            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Import_Master_Detail_Purchase(int import_Id)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"]))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("Import_Master_Detail_Select_Purchase", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Import_Id", import_Id > 0 ? import_Id : (object)DBNull.Value));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }

            return result;
        }

        #endregion


        #region Inward Detail

        public async Task<List<Dictionary<string, object>>> Get_Inward_Detail(string? Stock_ID, string? ID)
        {

            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"]))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("Get_Inward_Detail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Stock_ID", !string.IsNullOrEmpty(Stock_ID) ? Stock_ID : (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@ID", !string.IsNullOrEmpty(ID) ? ID : (object)DBNull.Value));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }

            return result;


        }

        #endregion

        #endregion

    }
}
