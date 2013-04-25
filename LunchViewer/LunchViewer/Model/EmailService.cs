using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Mail;
using System.Windows;
using LunchViewer.Interfaces;
using LunchViewer.Utils;

namespace LunchViewer.Model
{
    [Export(typeof(IEmailService))]
    class EmailService : IEmailService
    {
        [Import]
        private ISettings Settings { get; set; }

        public async void Send(DailyMenu daily_menu)
        {
            SmtpClient client = new SmtpClient("smtp.live.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("firstlast_lunchviewer@outlook.com", "FirstLast"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            var date = DateUtils.GetDateFormatted(Settings.Culture, daily_menu.Date);
            var Subject = "Daily lunch reminder for " + date;
            var body = daily_menu.Text;

            var message = new MailMessage
            {
                From = new MailAddress("LunchReminder@outlook.com", "Daily Lunch Reminder"),
                Subject = "Reminder for wednesday 24-04-2013",
                Body = "Mad og mad"
            };
            message.To.Add(new MailAddress(Settings.ReminderEmail));

            //try
            //{
            //    await client.SendMailAsync(message);
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //}
        }
    }
}