using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IV.Utilidades
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(subject, htmlMessage, email);
        }

        public Task Execute(string asunto, string mensaje, string email)
        {
            MailMessage mm = new MailMessage();
            mm.To.Add(email);
            mm.Subject = asunto;
            mm.Body = mensaje;
            mm.From = new MailAddress("no-reply@mail.com");
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.sendgrid.net");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("apikey", "************************");

            return smtp.SendMailAsync(mm);
        }
    }
}
