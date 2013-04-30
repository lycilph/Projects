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
        [Import]
        private ILocalizationService LocalizationService { get; set; }
        [Import]
        private IDialogService DialogService { get; set; }

        public async void Send(DailyMenu daily_menu)
        {
            // Create smtp (email) client with correct username etc.
            SmtpClient client = new SmtpClient("smtp.live.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("firstlast_lunchviewer@outlook.com", "FirstLast"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            // Create and localize strings
            var date = DateUtils.GetDateFormatted(Settings.Culture, daily_menu.Date);
            var subject = string.Format(LocalizationService.Localize("EmailSubject"), date);
            var display_name = LocalizationService.Localize("EmailDisplayName");

            // Create email itself
            var message = new MailMessage
            {
                From = new MailAddress("LunchReminder@outlook.com", display_name),
                Subject = subject,
                Body = daily_menu.Text,
            };
            message.To.Add(new MailAddress(Settings.ReminderEmail));

            // Try to send email
            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception e)
            {
                DialogService.ShowOkMessage(e.Message, "Error");
            }
        }
    }
}