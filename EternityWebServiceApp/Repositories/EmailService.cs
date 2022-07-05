using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace EternityWebServiceApp.Services
{
    static class EmailService
    {
        public static async Task SendEmailWithTemporaryPasswordAsync(string email, string newPassword)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("support.eternity.by", "di.dm.eternity@mail.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Запрос на восстановление пароля";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"Ваш временный пароль:<br><br>" +
                $"<b style='font-size: 14pt;'>{newPassword}</b><br><br>" +
                $"Смените его на свой собственный."
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 587);
                await client.AuthenticateAsync("di.dm.eternity@mail.ru", "KD14JajadCMSMa2wwNEs");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
