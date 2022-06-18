using finocred.BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace finocred.BusinessLayer.Services
{
    public class EmailSender : IEmailSender
    {
        public IConfiguration _configuration { get; }

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MailStatus SendEmail(string toEmail, string name, string subject, string content)
        {
            if (toEmail != null && name != null && subject != null)
            {
                var fromAddress = new MailAddress(_configuration["MailConfig:Email"], _configuration["MailConfig:Name"]);
                var toAddress = name == null ? new MailAddress(toEmail) : new MailAddress(toEmail, name);

                var smtp = new SmtpClient
                {
                    Host = _configuration["MailConfig:Host"],
                    Port = Convert.ToInt32(_configuration["MailConfig:Port"].ToString()),
                    EnableSsl = Convert.ToBoolean(_configuration["MailConfig:EnableSsl"].ToString()),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(_configuration["MailConfig:UseDefaultCredentials"].ToString()),
                    Credentials = new NetworkCredential(fromAddress.Address, _configuration["MailConfig:Password"].ToString())
                };

                MailMessage message = new MailMessage(fromAddress, toAddress);
                message.Subject = subject;
                message.Body = content;
                message.IsBodyHtml = true;

                //MailAddress bcc = new MailAddress(_configuration["MailSetting:CompanyEmail"]);
                //message.Bcc.Add(bcc);

                try
                {
                    smtp.Send(message);
                    return MailStatus.Success;
                }
                catch (Exception ex)
                {
                    var str = string.Format("Exception caught in CreateBccTestMessage(): {0}",
                        ex.ToString());
                }
            }

            return MailStatus.Failed;
        }

    }
}
