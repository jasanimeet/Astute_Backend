using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
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
            //    cValue = cValue.Replace("'", "•");//Tejas Add On 16/09/2011
            //    if (cValue.IsNumeric())
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

        //public static string Encrypt(string clearText)
        //{
        //    string encryptionKey = "F1597423-9641-4734-A9B4-14F6DB99B4C4";
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return clearText;
        //}
        //public static string Decrypt(string cipherText)
        //{
        //    string encryptionKey = "F1597423-9641-4734-A9B4-14F6DB99B4C4";
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                cs.Close();
        //            }
        //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    return cipherText;
        //}

        //public static string Encrypt(string cipherText)
        //{
        //    string key = "0123456789ABCDEF0123456789ABCDEF";

        //    byte[] encrypted;

        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(key);
        //        aes.Mode = CipherMode.CFB;

        //        aes.GenerateIV();

        //        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            ms.Write(aes.IV, 0, aes.IV.Length);

        //            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        //            using (StreamWriter sw = new StreamWriter(cs))
        //            {
        //                sw.Write(cipherText);
        //            }

        //            encrypted = ms.ToArray();
        //        }
        //    }
        //    string encryptedHex = BytesToHex(encrypted);
        //    return encryptedHex;
        //}

        //public static string Decrypt(string cipherText)
        //{
        //    string key = "0123456789ABCDEF0123456789ABCDEF";

        //    byte[] hexdata = HexToBytes(cipherText);

        //    byte[] decrypted;
        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(key);
        //        aes.Mode = CipherMode.CFB;

        //        // Extract the IV from the beginning of the cipherText
        //        byte[] iv = new byte[aes.IV.Length];
        //        Array.Copy(hexdata, 0, iv, 0, aes.IV.Length);
        //        aes.IV = iv;

        //        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        //        using (MemoryStream ms = new MemoryStream(hexdata, aes.IV.Length, cipherText.Length - aes.IV.Length))
        //        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        //        using (StreamReader sr = new StreamReader(cs))
        //        {
        //            decrypted = Encoding.UTF8.GetBytes(sr.ReadToEnd());
        //        }
        //    }
        //    string decryptedText = Encoding.UTF8.GetString(decrypted);
        //    return decryptedText;
        //}

        //static string BytesToHex(byte[] bytes)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (byte b in bytes)
        //    {
        //        sb.Append(b.ToString("X2"));
        //    }
        //    return sb.ToString();
        //}

        //static byte[] HexToBytes(string hex)
        //{
        //    byte[] bytes = new byte[hex.Length / 2];
        //    for (int i = 0; i < hex.Length; i += 2)
        //    {
        //        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        //    }
        //    return bytes;
        //}

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
    }
}
