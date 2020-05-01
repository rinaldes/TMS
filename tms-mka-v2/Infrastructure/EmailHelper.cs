using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Threading.Tasks;

namespace tms_mka_v2.Infrastructure
{
    public static class EmailHelper
    {
        public static void SendEmail(string EmailReceive, string SubjectEmail, string StringBody)
        {
            //send email
            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress("mail.kamanggala.com", SubjectEmail),
                new System.Net.Mail.MailAddress(EmailReceive)
            );
            var smtp = new SmtpClient
            {
                Host = "gator2024.hostgator.com",
                Port = 465,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            m.Subject = SubjectEmail;
            m.Body = StringBody;
            m.IsBodyHtml = true;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            smtp.Send(m);
        }
    }
}