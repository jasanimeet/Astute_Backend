using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IEmailSender
    {
        string ForgetPasswordBodyTemplate(string userEmail, string employeeName);
        void SendEmail(string toEmail = "", string externalLink = "", string subject = "", IFormFile formFile = null, string strBody = "");
        Task SendTestEmail(string toEmail = "", string subject = "", string strBody = "", int employeeId = 0);
    }
}
