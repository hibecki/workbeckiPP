using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PPtest.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            int res = -1;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("nokflyingtest@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = message;
                //SmtpServer.Port = 465;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("nokflyingtest@gmail.com", "N1994okok");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                res = 0;

                //try
                //{
                //MailMessage mail = new MailMessage();

                //SmtpClient SmtpServer = new SmtpClient("mail.palangpanya.com");
                //mail.From = new MailAddress("info@palangpanya.com");
                //mail.To.Add(email);
                //mail.Subject = subject;
                //mail.Body = message;
                //SmtpServer.Port = 25;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("info@palangpanya.com", "mfmHD9A2Ws");
                //SmtpServer.EnableSsl = false;

                //SmtpClient SmtpServer = new SmtpClient("smtp-mail.outlook.com");
                //mail.From = new MailAddress("b170320162016@outlook.com");
                //mail.To.Add(email);
                //mail.Subject = subject;
                //mail.Body = message;
                //SmtpServer.Port = 587;
                //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //SmtpServer.UseDefaultCredentials = false;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("b170320162016@outlook.com", "pp27062016");
                //SmtpServer.EnableSsl = true;

                //SmtpServer.SendMailAsync(mail);
                //res = 0;



                //}
                //catch (Exception ex)
                //{
                //    res = ex.HResult;
                //}

                using (MailMessage m = new MailMessage("info@palangpanya.com", email))
                {
                    m.Subject = subject;
                    m.Body = message;
                    using (SmtpClient client = new SmtpClient
                    {
                        EnableSsl = false,
                        Host = "mail.palangpanya.com",
                        Port = 25,
                        Credentials = new NetworkCredential("info@palangpanya.com", "mfmHD9A2Ws")
                    })
                    {
                        client.SendMailAsync(m);
                        res = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.HResult;
            }

            return Task.FromResult(res);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
