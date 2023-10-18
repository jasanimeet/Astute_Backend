using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Mail;
using System.Net;
using System;
using System.Text;
using astute.CoreServices;
using System.Linq;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class EmailSender : IEmailSender
    {
        #region Fields
        private readonly IConfiguration _configuration;
        private readonly IEmployeeService _employeeService;
        #endregion

        #region Ctor
        public EmailSender(IConfiguration configuration,
            IEmployeeService employeeService)
        {
            _configuration = configuration;
            _employeeService = employeeService;
        }
        #endregion

        #region Methods
        public string ForgetPasswordBodyTemplate(string userName, string employeeName)
        {
            var baseUrl = _configuration["FrontUrl"];
            var redirectUrl = baseUrl + "/reset-password?code=" + CoreService.Encrypt(userName);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"Dear " + employeeName + ",");
            sb.Append(@"You have requested to change your password. You can do this through the link below or copy paste the url in browser if the link is not visible.,");
            sb.Append(@"'Change my password'");
            sb.Append(@"<a href='" + redirectUrl + "'> Click Here </a>");
            sb.Append(@"If you didn't request this, please ignore this email.");

            return sb.ToString();
        }
        public void SendEmail(string toEmail = "", string externalLink = "", string subject = "", IFormFile formFile = null, string strBody = "")
        {
            MailMessage mailMessage = new MailMessage(_configuration["EmailSetting:FromEmail"], toEmail);
            mailMessage.Subject = subject;
            mailMessage.Body = strBody;
            if (formFile != null && formFile.Length > 0)
            {
                string fileName = Path.GetFileName(formFile.FileName);
                mailMessage.Attachments.Add(new Attachment(formFile.OpenReadStream(), fileName));
            }
            mailMessage.IsBodyHtml = true;

            using (SmtpClient smptClient = new SmtpClient())
            {
                smptClient.Host = _configuration["EmailSetting:SmtpHost"];
                smptClient.EnableSsl = true;
                smptClient.UseDefaultCredentials = false;
                smptClient.Credentials = new NetworkCredential(_configuration["EmailSetting:FromEmail"], _configuration["EmailSetting:FromPassword"]);
                smptClient.Port = Convert.ToInt32(_configuration["EmailSetting:SmtpPort"]);
                smptClient.Send(mailMessage);
            }
        }
        public async Task SendTestEmail(string toEmail = "", string subject = "", string strBody = "", int employeeId = 0)
        {
            var employeeEmail = await _employeeService.GetEmployeeMail(employeeId);
            var mailSetting = employeeEmail.FirstOrDefault();

            string fromEmail = !string.IsNullOrEmpty(mailSetting.Email_id) ? mailSetting.Email_id : _configuration["EmailSetting:FromEmail"];
            string authPassword = !string.IsNullOrEmpty(mailSetting.Email_Password) ? mailSetting.Email_Password : _configuration["EmailSetting:FromPassword"];
            string smtpHost = !string.IsNullOrEmpty(mailSetting.SMTP_Server_Address) ? mailSetting.SMTP_Server_Address : _configuration["EmailSetting:SmtpHost"];
            int smtpPort = mailSetting.SMTP_Port > 0 ? mailSetting.SMTP_Port : Convert.ToInt32(_configuration["EmailSetting:SmtpPort"]);
            bool enableSSL = mailSetting.Enable_SSL;

            MailMessage mailMessage = new MailMessage(fromEmail, toEmail);
            mailMessage.Subject = subject;
            mailMessage.Body = strBody;
            mailMessage.IsBodyHtml = true;

            using (SmtpClient smptClient = new SmtpClient())
            {
                smptClient.Host = smtpHost;
                smptClient.EnableSsl = enableSSL;
                smptClient.UseDefaultCredentials = false;
                smptClient.Credentials = new NetworkCredential(fromEmail, authPassword);
                smptClient.Port = smtpPort;
                smptClient.Send(mailMessage);
            }
        }
        #endregion
    } 
}
