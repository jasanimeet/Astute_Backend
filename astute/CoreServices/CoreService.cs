using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace astute.CoreServices
{
    public class CoreService
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
            if (File.Exists(file_Path))
            {
                File.Delete(file_Path);
            }
        }
        public static void Remove_Files_From_Folder(IList<string> files, string root_folder)
        {
            foreach (var file in files)
            {
                if (File.Exists(Path.Combine(root_folder, file)))
                {
                    File.Delete(file);
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

                // Get the response and download the file
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream fileStream = File.Create(localFolderPath))
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
        public static string Splite_Supplier_Stock_Measurement(string expression, string dimension)
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
                    return values[0].ToString();
                case "width":
                    return values[1].ToString();
                case "depth":
                    return values[2].ToString();
                default:
                    throw new ArgumentException("Invalid dimension specified.");
            }
        }

    }
}
