using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPcore.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailWithAttachment(string email, string subject, string message, List<string> attachedPath);
    }
}
