using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finocred.BusinessLayer.Interfaces
{
    public interface IEmailSender
    {
        //Task<MailStatus> SendEmailAsync(string email, string name, string subject, string htmlMessage);
        MailStatus SendEmail(string email, string name, string subject, string htmlMessage);
    }
}

public enum MailStatus
{
    Success,
    NotConfigure,
    Failed
}