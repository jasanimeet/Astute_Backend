using astute.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IEmailSender
    {
        string ForgetPasswordBodyTemplate(string userEmail, string employeeName);
        void SendEmail(string toEmail = "", string externalLink = "", string subject = "", IFormFile formFile = null, string strBody = "");
        void Send_Stock_Email(string toEmail = "", string externalLink = "", string subject = "", IFormFile formFile = null, string strBody = "", int user_Id = 0, Employee_Mail employee_Mail = null);
        Task SendTestEmail(string toEmail = "", string subject = "", string strBody = "", int employeeId = 0);
        public void SendEmail(List<string> toEmails, string subject, string body, List<string> ccEmails = null, List<string> bccEmails = null, List<IFormFile> formFiles = null);
    }
}
