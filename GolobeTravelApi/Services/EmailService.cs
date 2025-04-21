using Resend;

namespace GolobeTravelApi.Services
{
    public class EmailService
    {
        private readonly IResend _resend;

        public EmailService(IResend resend)
        {
            _resend = resend;
        }

        public async Task SendWelcomeMessage(string toEmail, string name)
        {

            string html = await File.ReadAllTextAsync("Helpers/EmailHtmlTemplates/WelcomeEmail.html");
            html = html.Replace("{{name}}", name);
        

            var message = new EmailMessage
            {
                From = "no-reply@khomotso.dev",
                Subject = "Welcome to Golobe Travel!",
                HtmlBody = html,
                TextBody = $"Hi {name}, welcome to Golobe Travel! Start exploring at golobetravel.com."
            };
            message.To.Add(toEmail);

            try
            {
                await _resend.EmailSendAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send welcome email: {ex.Message}");
            }
        }

        public async Task SendBookingConfirmationEmail(string toEmail, string name, string hotelName, DateTime checkin, DateTime checkout, decimal total, int rooms, int adults, int children)
        {

            string html = await File.ReadAllTextAsync("Helpers/EmailHtmlTemplates/BookingConfirmation.html");
            html = html.Replace("{{name}}", name)
                    .Replace("{{hotelName}}", hotelName)
                    .Replace("{{checkin}}", checkin.ToString())
                    .Replace("{{checkout}}", checkout.ToString())
                    .Replace("{{room}}", $"{rooms} room(s)")
                    .Replace("{{adults}}", adults.ToString())
                    .Replace("{{children}}", children.ToString())
                    .Replace("{{total}}", $"R{total:N2}");

            var message = new EmailMessage
            {
                From = "no-reply@khomotso.dev",
                Subject = "Golobe Booking Confirmation",
                HtmlBody = html,
                TextBody = $"Hi {name}, your stay at {hotelName} is confirmed from {checkin:yyyy-MM-dd} to {checkout:yyyy-MM-dd}. Total cost: ZAR {total:F2}."
            };

            message.To.Add(toEmail);

            try
            {
                await _resend.EmailSendAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send booking confirmation email: {ex.Message}");
            }
        }
    }
}
