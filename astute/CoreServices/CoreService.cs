using astute.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.FileIO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static NPOI.HSSF.UserModel.HeaderFooter;

namespace astute.CoreServices
{
    public static class CoreService
    {
        private static Byte[] Key_64 = { 42, 16, 93, 156, 78, 4, 218, 32 };
        private static Byte[] Iv_64 = { 55, 103, 246, 79, 36, 99, 167, 3 };

        public static string Encrypt(string cValue, bool isFile = false)
        {
            string cAsVal = Decrypt(cValue);
            if (!cAsVal.Equals(cValue))
                return cValue;
            //if (!isFile)
            //{
            //    cValue = cValue.Replace("'", "•");
            //    if (IsNumeric(cValue))
            //        cValue = "A_°" + cValue;
            //}
            DESCryptoServiceProvider CryptoProvidor = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, CryptoProvidor.CreateEncryptor(Key_64, Iv_64), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cs);
            sw.Write(cValue);
            sw.Flush();
            cs.FlushFinalBlock();
            ms.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length));
        }
        //public static bool IsNumeric(this string value)
        //{
        //    double number;
        //    return double.TryParse(value, out number);
        //}
        public static string Decrypt(string cValue, bool isFile = false)
        {
            try
            {
                DESCryptoServiceProvider CryptoProvidor = new DESCryptoServiceProvider();
                Byte[] buf = new byte[cValue.Length];
                buf = Convert.FromBase64String(cValue);
                MemoryStream ms = new MemoryStream(buf);
                CryptoStream cs = new CryptoStream(ms, CryptoProvidor.CreateDecryptor(Key_64, Iv_64), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                string cRetVal = sr.ReadToEnd();

                //if (!isFile)
                //{
                //    cRetVal = cRetVal.Replace("•", "'");//Tejas Add On 16/09/2011
                //    if (cRetVal.StartsWith("A_°"))
                //        cRetVal = cRetVal.Replace("A_°", "");
                //}
                return cRetVal;
            }
            catch //(Exception ex)
            {
                //MessageBox.Show("Error in Decryptstring : " + ex.Message);
            }
            return cValue;
        }
        public static string ConvertModelListToJson<T>(List<T> data)
        {
            return System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        }
        public static string ConvertModelToJson<T>(T data)
        {
            return System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        }
        public async static Task<string> GetIP_Address(IHttpContextAccessor _httpContextAccessor)
        {
            string ip_Address = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (String.IsNullOrEmpty(ip_Address))
                ip_Address = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];

            if (String.IsNullOrEmpty(ip_Address))
                ip_Address = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrEmpty(ip_Address))
                ip_Address = _httpContextAccessor.HttpContext.Connection.LocalIpAddress.ToString();

            if (string.IsNullOrEmpty(ip_Address) || ip_Address.Trim() == "::1")
            {
                ip_Address = string.Empty;
            }

            if (Dns.GetHostEntry(Dns.GetHostName()).AddressList
                    .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .ToString() == ip_Address)
            {
                ip_Address = string.Empty;
            }

            if (string.IsNullOrEmpty(ip_Address))
            {
                try
                {
                    string pattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync("http://checkip.dyndns.org");
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            Match match = Regex.Match(content, pattern);
                            if (match.Success)
                            {
                                ip_Address = match.Value;
                            }
                        }
                    }
                }
                catch
                {
                    return null;
                }
            }
            return ip_Address;
        }
        public static (SqlParameter, SqlParameter, SqlParameter, SqlParameter, SqlParameter) Get_SqlParameter_Values(int employeeId, string ipAddress, DateTime traceDate, TimeSpan traceTime, string recordType)
        {
            var paramEmployeeId = new SqlParameter("@Employee_Id", employeeId);
            var paramIpAddress = new SqlParameter("@IP_Address", ipAddress);
            var paramTraceDate = new SqlParameter("@Trace_Date", traceDate);
            var paramTraceTime = new SqlParameter("@Trace_Time", traceTime);
            var paramRecordType = new SqlParameter("@RecordType", recordType);

            return (paramEmployeeId, paramIpAddress, paramTraceDate, paramTraceTime, paramRecordType);
        }
        public static bool Enable_Trace_Records(IConfiguration _configuration)
        {
            bool isEnable = false;
            string strEnableTraceRecord = _configuration["Enable_Trace_Record"];
            if (!string.IsNullOrEmpty(strEnableTraceRecord))
                isEnable = Convert.ToBoolean(_configuration["Enable_Trace_Record"]);

            return isEnable;
        }
        public static void Remove_File_From_Folder(string file_Path)
        {
            if (System.IO.File.Exists(file_Path))
            {
                System.IO.File.Delete(file_Path);
            }
        }
        public static void Remove_Files_From_Folder(IList<string> files, string root_folder)
        {
            foreach (var file in files)
            {
                if (System.IO.File.Exists(Path.Combine(root_folder, file)))
                {
                    System.IO.File.Delete(file);
                }
            }
        }
        public static void Ftp_File_Download(string ftpServer, string username, string password, int ftpPort, string remoteFilePath, string localFolderPath)
        {
            try
            {
                // Build the FTP URL including port number
                string ftpUrl = $"ftp://{ftpServer}:{ftpPort}{"/"}{remoteFilePath}";

                // Create the FTP request
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Credentials = new NetworkCredential(username, password);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                //Get the response and download the file
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream fileStream = System.IO.File.Create(localFolderPath))
                {
                    responseStream.CopyTo(fileStream);
                    Console.WriteLine($"File downloaded to {localFolderPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public static string Get_Authorization_Token(IHttpContextAccessor _httpContextAccessor)
        {
            var bearerToken = string.Empty;
            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                bearerToken = authHeader.ToString().Split(' ')[1];
            }
            return bearerToken;
        }
        public static string RemoveNonNumericAndDotAndNegativeCharacters(string input)
        {
            string pattern = "[^0-9.-]";
            Regex regex = new Regex(pattern);
            string result = regex.Replace(input, "");
            result = (result == "-" ? "" : result);
            return result;
        }
        public static string GetCertificateNoOrUrl(string input, bool IsUrl)
        {
            string[] certificate = input.Split(",");
            string data = certificate.Length > 1 ? (IsUrl == true ? certificate[1] : certificate[0]) : input;
            return data.Trim();
        }
        public static DataTable Convert_File_To_DataTable(string filetype, string connString, string sheetNames)
        {
            //DataTable mergedTable = new DataTable();

            //var sheet_Names = sheetNames.Split(",");

            //if (filetype == ".xls" || filetype == ".xlsx")
            //{
            //    using (OleDbConnection connection = new OleDbConnection(connString))
            //    {
            //        connection.Open();

            //        //Fetch the first sheet to initialize the mergedTable
            //        OleDbDataAdapter initialAdapter = new OleDbDataAdapter($"SELECT * FROM [{sheet_Names.First()}$]", connection);
            //        initialAdapter.Fill(mergedTable);

            //        // Merge the remaining sheets
            //        foreach (string sheetName in sheet_Names.Skip(1))
            //        {
            //            OleDbDataAdapter adapter = new OleDbDataAdapter($"SELECT * FROM [{sheetName}$]", connection);
            //            DataTable sheetTable = new DataTable(sheetName); // Use sheet name as table name

            //            adapter.Fill(sheetTable);
            //            mergedTable = UnionTables(mergedTable, sheetTable);
            //            // Merge columns if not already present
            //        }
            //        connection.Close();
            //    }
            //}

            DataTable mergedTable = new DataTable();

            var sheet_Names = sheetNames.Split(",");

            if (filetype == ".xls" || filetype == ".xlsx")
            {
                using (OleDbConnection connection = new OleDbConnection(connString))
                {
                    connection.Open();

                    // Fetch the first sheet to initialize the mergedTable
                    OleDbDataAdapter initialAdapter = new OleDbDataAdapter($"SELECT * FROM [{sheet_Names.First()}$]", connection);
                    initialAdapter.Fill(mergedTable);

                    // Trim column names
                    mergedTable.Columns.Cast<DataColumn>().ToList().ForEach(column => column.ColumnName = column.ColumnName.Trim());

                    // Merge the remaining sheets
                    foreach (string sheetName in sheet_Names.Skip(1))
                    {
                        OleDbDataAdapter adapter = new OleDbDataAdapter($"SELECT * FROM [{sheetName}$]", connection);
                        DataTable sheetTable = new DataTable(sheetName); // Use sheet name as table name

                        adapter.Fill(sheetTable);

                        // Trim column names in the sheetTable
                        foreach (DataColumn column in sheetTable.Columns)
                        {
                            column.ColumnName = column.ColumnName.Trim();
                        }

                        mergedTable = UnionTables(mergedTable, sheetTable);
                        // Merge columns if not already present
                    }
                    connection.Close();
                }
            }

            else if (filetype == ".csv")
            {
                string[] fields = null;
                using (TextFieldParser parser = new TextFieldParser(connString))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    string[] headers = parser.ReadFields();
                    mergedTable.Columns.AddRange(headers.Select(header => new DataColumn(header.Trim())).ToArray());

                    while (!parser.EndOfData)
                    {
                        try
                        {
                            fields = parser.ReadFields();
                        }
                        catch (MalformedLineException ex)
                        {
                            fields = null;
                        }

                        if (fields != null)
                        {
                            DataRow row = mergedTable.NewRow();
                            for (int i = 0; i < mergedTable.Columns.Count; i++)
                            {
                                if (fields.Length > i)
                                {
                                    row[i] = fields[i];
                                }
                            }
                            mergedTable.Rows.Add(row);
                        }
                    }
                }
            }

            return mergedTable;
        }
        static DataTable UnionTables(DataTable table1, DataTable table2)
        {
            // Use LINQ Union to combine rows and remove duplicates
            var query = table1.AsEnumerable().Union(table2.AsEnumerable(), DataRowComparer.Default);

            // Create a new DataTable with the structure of the source tables
            DataTable resultTable = query.CopyToDataTable();

            return resultTable;
        }
        public static (bool, int) CheckDataInFirstTenRowsAndColumns(ExcelWorksheet worksheet)
        {
            var dataExist = false;
            int rowCount = Math.Min(worksheet.Dimension.End.Row, 15);
            int colCount = Math.Min(worksheet.Dimension.End.Column, 15);
            int row_cnt = 0;
            int col_cnt = 0;
            for (int row = 1; row <= rowCount; row++)
            {
                row_cnt = row_cnt + 1;
                for (int col = 1; col <= colCount; col++)
                {
                    var cell = worksheet.Cells[row, col];
                    if (!string.IsNullOrEmpty(cell.Text))
                    {
                        col_cnt = col_cnt + 1;
                        dataExist = true;
                    }
                    else
                    {
                        col_cnt = 0;
                        dataExist = false;
                    }
                    if (dataExist && col_cnt == 15)
                    {
                        goto dataFound;
                    }
                }
            }
        dataFound:
            return (dataExist, row_cnt);
        }
        public static (bool, int) CheckDataInFirstTenRowsAndColumns(HSSFSheet sheet)
        {
            var dataExist = false;
            int rowCount = Math.Min(sheet.LastRowNum + 1, 15);
            int colCount = 15;
            int row_cnt = 0;
            int col_cnt = 0;

            for (int row = 1; row < rowCount; row++)
            {
                row_cnt = row_cnt + 1;
                HSSFRow currentRow = (HSSFRow)sheet.GetRow(row);
                if (currentRow != null)
                {
                    for (int col = 0; col < colCount; col++)
                    {
                        HSSFCell cell = (HSSFCell)currentRow.GetCell(col);
                        //if (cell != null && !string.IsNullOrEmpty(cell.StringCellValue))
                        if (cell != null)
                        {
                            col_cnt = col_cnt + 1;
                            dataExist = true;
                        }
                        else
                        {
                            col_cnt = 0;
                            dataExist = false;
                        }
                        if (dataExist && col_cnt == 15)
                        {
                            goto dataFound;
                        }
                    }
                }
            }
        dataFound:
            return (dataExist, row_cnt);
        }
        public static string Split_Supplier_Stock_Measurement(string expression, string dimension)
        {

            // Define a regular expression pattern to capture numeric values
            string pattern = @"[-+]?\d*\.\d+|[-+]?\d+";

            // Use Regex.Matches to find all matches
            MatchCollection matches = Regex.Matches(expression, pattern);

            // Extract numeric values and store them in an array
            double[] values = matches.Cast<Match>().Select(match => Convert.ToDouble(match.Value)).ToArray();

            // Sort the values in descending order
            Array.Sort(values, (a, b) => b.CompareTo(a));

            // Determine the dimension to return
            switch (dimension.ToLower())
            {
                case "length":
                    return values.Length > 0 ? values[0].ToString() : "";
                case "width":
                    return values.Length > 1 ? values[1].ToString() : "";
                case "depth":
                    return values.Length > 2 ? values[2].ToString() : "";
                default:
                    throw new ArgumentException("Invalid dimension specified.");
            }

        }
        public static (string, string) Table_And_Side_White(string strValue)
        {
            if (!string.IsNullOrEmpty(strValue))
            {
                if (strValue == "NONE")
                {
                    return ("NN", "NN");
                }
                else
                {
                    string[] strArray = strValue.Split(',');

                    Int32 ok_Len = Convert.ToInt32(strArray.Length) - 1;
                    Int32 Exist_Len = 0;

                    if (strArray[0] == "T1" || strArray[0] == "T2")
                    {
                        if (strArray.Length == 2)
                        {
                            foreach (string str in strArray)
                            {
                                if (str.Trim() == "TS" || str.Trim() == "TC")
                                {
                                    Exist_Len += 1;
                                }
                            }
                            if (Exist_Len == ok_Len)
                            {
                                return ("T1", "NN");
                            }

                            foreach (string str in strArray)
                            {
                                if (str.Trim() == "CC" || str.Trim() == "CG")
                                {
                                    Exist_Len += 1;
                                }
                            }
                            if (Exist_Len == ok_Len)
                            {
                                return ("NN", "C1");
                            }
                        }
                        else if (strArray.Length == 3)
                        {
                            foreach (string str in strArray)
                            {
                                if (str.Trim() == "TS" || str.Trim() == "TC" || str.Trim() == "CC" || str.Trim() == "CG")
                                {
                                    Exist_Len += 1;
                                }
                            }
                            if (Exist_Len == ok_Len)
                            {
                                return ("T1", "C1");
                            }
                        }
                    }
                    else if (strArray[0] == "T3")
                    {
                        if (strArray.Length == 2)
                        {
                            foreach (string str in strArray)
                            {
                                if (str.Trim() == "TS" || str.Trim() == "TC")
                                {
                                    Exist_Len += 1;
                                }
                            }
                            if (Exist_Len == ok_Len)
                            {
                                return ("T2", "NN");
                            }

                            foreach (string str in strArray)
                            {
                                if (str.Trim() == "CC" || str.Trim() == "CG")
                                {
                                    Exist_Len += 1;
                                }
                            }
                            if (Exist_Len == ok_Len)
                            {
                                return ("NN", "C2");
                            }
                        }
                        else if (strArray.Length == 3)
                        {
                            foreach (string str in strArray)
                            {
                                if (str.Trim() == "TS" || str.Trim() == "TC" || str.Trim() == "CC" || str.Trim() == "CG")
                                {
                                    Exist_Len += 1;
                                }
                            }
                            if (Exist_Len == ok_Len)
                            {
                                return ("T2", "C2");
                            }
                        }
                    }
                }
            }
            return (null, null);
        }
        public static (string, string) Table_And_Side_Black(string strValue)
        {
            if (!string.IsNullOrEmpty(strValue))
            {
                if (strValue == "NONE")
                {
                    return ("NN", "NN");
                }
                else
                {
                    string[] strArray = strValue.Split(',');

                    Int32 ok_Len = Convert.ToInt32(strArray.Length) - 1;
                    Int32 Exist_Len = 0;

                    if (strArray[0] == "N1" || strArray[0] == "N2")
                    {
                        if (strArray.Length == 2)
                        {
                            foreach (string str in strArray)
                            {
                                if (str.Trim() == "TS" || str.Trim() == "TC")
                                {
                                    Exist_Len += 1;
                                }
                            }
                            if (Exist_Len == ok_Len)
                            {
                                return ("BT1", "NN");
                            }

                            foreach (string str in strArray)
                            {
                                if (str.Trim() == "CC" || str.Trim() == "CG")
                                {
                                    Exist_Len += 1;
                                }
                            }
                            if (Exist_Len == ok_Len)
                            {
                                return ("NN", "BC1");
                            }
                        }
                        else if (strArray.Length == 3)
                        {
                            foreach (string str in strArray)
                            {
                                if (str.Trim() == "TS" || str.Trim() == "TC" || str.Trim() == "CC" || str.Trim() == "CG")
                                {
                                    Exist_Len += 1;
                                }
                            }
                            if (Exist_Len == ok_Len)
                            {
                                return ("BT1", "BC1");
                            }
                        }
                    }
                    else if (strArray[0] == "N3")
                    {
                        if (strArray.Length == 2)
                        {
                            foreach (string str in strArray)
                            {
                                if (str.Trim() == "TS" || str.Trim() == "TC")
                                {
                                    Exist_Len += 1;
                                }
                            }
                            if (Exist_Len == ok_Len)
                            {
                                return ("BT2", "NN");
                            }

                            foreach (string str in strArray)
                            {
                                if (str.Trim() == "CC" || str.Trim() == "CG")
                                {
                                    Exist_Len += 1;
                                }
                            }
                            if (Exist_Len == ok_Len)
                            {
                                return ("NN", "BC2");
                            }
                        }
                        else if (strArray.Length == 3)
                        {
                            foreach (string str in strArray)
                            {
                                if (str.Trim() == "TS" || str.Trim() == "TC" || str.Trim() == "CC" || str.Trim() == "CG")
                                {
                                    Exist_Len += 1;
                                }
                            }
                            if (Exist_Len == ok_Len)
                            {
                                return ("BT2", "BC2");
                            }
                        }
                    }
                }
            }
            return (null, null);
        }
        public static DataTable ToDataTable<T>(this T[] array)
        {
            DataTable dataTable = new DataTable();

            // Assuming T is a reference type, you can use its properties as columns
            if (array.Length > 0)
            {
                PropertyInfo[] properties = array[0].GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    dataTable.Columns.Add(property.Name, property.PropertyType);
                }

                foreach (T item in array)
                {
                    DataRow row = dataTable.NewRow();

                    foreach (PropertyInfo property in properties)
                    {
                        row[property.Name] = property.GetValue(item);
                    }

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }
        public static DataTable ConvertToDataTable(List<Dictionary<string, object>> rowsData)
        {
            DataTable dataTable = new DataTable();

            if (rowsData.Count > 0)
            {
                // Create columns
                foreach (string columnName in rowsData[0].Keys)
                {
                    dataTable.Columns.Add(columnName);
                }

                // Add rows
                foreach (Dictionary<string, object> rowData in rowsData)
                {
                    DataRow newRow = dataTable.NewRow();
                    foreach (KeyValuePair<string, object> kvp in rowData)
                    {
                        newRow[kvp.Key] = kvp.Value;
                    }
                    dataTable.Rows.Add(newRow);
                }
            }

            return dataTable;
        }
        static double ExtractNumericValue(string input)
        {
            Regex regex = new Regex(@"\d+(\.\d+)?");
            Match match = regex.Match(input);

            if (match.Success)
            {
                return double.Parse(match.Value);
            }

            return double.MinValue; // or some other appropriate default value
        }
        public static string ExtractStringWithLargestNumericValue(params string[] strings)
        {
            string result = null;
            double maxValue = double.MinValue;

            foreach (string input in strings)
            {
                double numericValue = ExtractNumericValue(input);

                if (numericValue > maxValue)
                {
                    maxValue = numericValue;
                    result = input;
                }
            }

            return result ?? "";
        }
    }
}
