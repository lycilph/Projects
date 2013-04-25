using System;
using System.Net;
using System.Net.Mail;

namespace EmailTest
{
    class Program
    {
        static void Main()
        {
            SmtpClient client = new SmtpClient("smtp.live.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("firstlast_lunchviewer@outlook.com", "FirstLast"),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true
                };

            var message = new MailMessage
                {
                    From = new MailAddress("LunchReminder@outlook.com", "Daily Lunch Reminder"),
                    Subject = "Reminder for wednesday 24-04-2013",
                    Body = "Mad og mad"
                };
            message.To.Add(new MailAddress("mml@oticon.dk"));

            try
            {
                client.Send(message);
                Console.WriteLine("Message sent");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();

        }
    }
}
