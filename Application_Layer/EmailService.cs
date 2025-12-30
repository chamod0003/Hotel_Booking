using Application_Layer.Interface;
using Domain_Layer.Models.Entity;
using Infrastructure_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }

        public async Task SendBookingCancellationEmailAsync(Booking booking)
        {
            var subject = $"Booking Cancelled - {booking.BookingReference}";

            var body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 2px solid #dc2626; border-radius: 10px; }}
        .header {{ background: linear-gradient(135deg, #dc2626 0%, #991b1b 100%); color: white; padding: 30px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ padding: 30px; }}
        .cancel-box {{ background-color: #fee2e2; padding: 25px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #dc2626; }}
        .footer {{ text-align: center; color: #6b7280; font-size: 12px; margin-top: 30px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1 style='margin: 0; font-size: 28px;'>Booking Cancelled</h1>
        </div>
        
        <div class='content'>
            <p style='font-size: 16px;'>Dear <strong>{booking.User.FullName}</strong>,</p>
            
            <p>Your booking has been cancelled as per your request.</p>
            
            <div class='cancel-box'>
                <p style='margin: 10px 0; color: #991b1b;'><strong>Booking Reference:</strong> {booking.BookingReference}</p>
                <p style='margin: 10px 0; color: #991b1b;'><strong>Hotel:</strong> {booking.Hotel.HotelName}</p>
                <p style='margin: 10px 0; color: #991b1b;'><strong>Cancellation Date:</strong> {booking.CancellationDate:MMMM dd, yyyy}</p>
                {(string.IsNullOrEmpty(booking.CancellationReason) ? "" : $@"
                <p style='margin: 10px 0; color: #991b1b;'><strong>Reason:</strong> {booking.CancellationReason}</p>
                ")}
                {(booking.RefundAmount.HasValue ? $@"
                <p style='margin: 10px 0; color: #991b1b;'><strong>Refund Amount:</strong> LKR {booking.RefundAmount:N2}</p>
                " : "")}
            </div>
            
            <p>If you have any questions or need assistance with rebooking, please contact us.</p>
        </div>
        
        <div class='footer'>
            <p><strong>Hotel Service</strong> - Your Trusted Booking Partner</p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(booking.User.Email, subject, body);
        }

        public async Task SendBookingConfirmationEmailAsync(Booking booking)
        {
            var subject = $"✓ Booking Confirmation - {booking.BookingReference}";

            var body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 2px solid #2563eb; border-radius: 10px; }}
        .header {{ background: linear-gradient(135deg, #2563eb 0%, #1e40af 100%); color: white; padding: 30px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ padding: 30px; background-color: #ffffff; }}
        .details {{ background-color: #f9fafb; padding: 25px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #2563eb; }}
        .detail-row {{ display: flex; justify-content: space-between; padding: 12px 0; border-bottom: 1px solid #e5e7eb; }}
        .detail-label {{ color: #6b7280; font-weight: 500; }}
        .detail-value {{ color: #1a1a1a; font-weight: 600; }}
        .info-box {{ background-color: #eff6ff; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #3b82f6; }}
        .footer {{ text-align: center; color: #6b7280; font-size: 12px; margin-top: 30px; padding-top: 20px; border-top: 1px solid #e5e7eb; }}
        .highlight {{ color: #2563eb; font-size: 24px; font-weight: bold; }}
        .status-badge {{ background-color: #d1fae5; color: #065f46; padding: 6px 12px; border-radius: 4px; font-weight: bold; display: inline-block; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1 style='margin: 0; font-size: 28px;'>🎉 Booking Confirmed!</h1>
            <p style='margin: 10px 0 0 0; font-size: 14px; opacity: 0.9;'>Thank you for choosing Hotel Service</p>
        </div>
        
        <div class='content'>
            <p style='font-size: 16px;'>Dear <strong>{booking.User.FullName}</strong>,</p>
            
            <p>Your hotel booking has been successfully confirmed! We're excited to welcome you.</p>
            
            <div class='details'>
                <div class='detail-row'>
                    <span class='detail-label'>Booking Reference:</span>
                    <span class='detail-value' style='color: #2563eb;'>{booking.BookingReference}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Hotel:</span>
                    <span class='detail-value'>{booking.Hotel.HotelName}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Room Type:</span>
                    <span class='detail-value'>{booking.RoomType.RoomTypeName}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Check-in Date:</span>
                    <span class='detail-value'>{booking.CheckInDate:dddd, MMMM dd, yyyy}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Check-out Date:</span>
                    <span class='detail-value'>{booking.CheckOutDate:dddd, MMMM dd, yyyy}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Duration:</span>
                    <span class='detail-value'>{booking.TotalNights} night(s)</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Guests:</span>
                    <span class='detail-value'>{booking.NumberOfAdults} Adult(s), {booking.NumberOfChildren} Child(ren)</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Rooms:</span>
                    <span class='detail-value'>{booking.NumberOfRooms} room(s)</span>
                </div>
                <div class='detail-row' style='border-bottom: none; margin-top: 15px; padding-top: 15px; border-top: 2px solid #2563eb;'>
                    <span class='detail-label' style='font-size: 18px;'>Total Amount:</span>
                    <span class='highlight'>LKR {booking.TotalAmount:N2}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Booking Status:</span>
                    <span class='status-badge'>{booking.BookingStatus}</span>
                </div>
                <div class='detail-row' style='border-bottom: none;'>
                    <span class='detail-label'>Payment Status:</span>
                    <span style='background-color: #fef3c7; color: #92400e; padding: 6px 12px; border-radius: 4px; font-weight: bold; display: inline-block;'>{booking.PaymentStatus}</span>
                </div>
            </div>
            
            <div class='info-box'>
                <p style='margin: 0 0 10px 0; font-weight: bold; color: #1e40af; font-size: 16px;'>📌 Important Information</p>
                <ul style='color: #1e40af; margin: 10px 0; padding-left: 20px; line-height: 2;'>
                    <li>Check-in time: <strong>2:00 PM</strong></li>
                    <li>Check-out time: <strong>11:00 AM</strong></li>
                    <li>Please bring a valid <strong>ID</strong> for check-in</li>
                    <li>Cancellation policy applies as per terms</li>
                    <li>For any queries, contact the hotel directly</li>
                </ul>
            </div>
            
            {(string.IsNullOrEmpty(booking.SpecialRequests) ? "" : $@"
            <div style='background-color: #fef3c7; padding: 15px; border-radius: 8px; margin: 20px 0;'>
                <p style='margin: 0; font-weight: bold; color: #92400e;'>💬 Your Special Request:</p>
                <p style='margin: 10px 0 0 0; color: #92400e;'>{booking.SpecialRequests}</p>
            </div>
            ")}
            
            <p style='font-size: 16px; margin-top: 30px;'>We look forward to welcoming you!</p>
            
            <p style='color: #6b7280;'>If you have any questions, please don't hesitate to contact us.</p>
        </div>
        
        <div class='footer'>
            <p style='margin: 5px 0;'><strong style='color: #2563eb; font-size: 16px;'>Hotel Service</strong></p>
            <p style='margin: 5px 0;'>Your Trusted Booking Partner</p>
            <p style='margin: 10px 0 0 0; font-size: 11px;'>This is an automated message. Please do not reply to this email.</p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(booking.User.Email, subject, body);
        }

        public async Task SendPaymentConfirmationEmailAsync(Payment payment, Booking booking)
        {
            var subject = $"✓ Payment Successful - {booking.BookingReference}";

            var body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 2px solid #10b981; border-radius: 10px; }}
        .header {{ background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: white; padding: 30px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ padding: 30px; }}
        .payment-box {{ background-color: #d1fae5; padding: 25px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #10b981; }}
        .detail-row {{ display: flex; justify-content: space-between; padding: 12px 0; border-bottom: 1px solid #6ee7b7; }}
        .footer {{ text-align: center; color: #6b7280; font-size: 12px; margin-top: 30px; }}
        .highlight {{ color: #059669; font-size: 28px; font-weight: bold; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div style='font-size: 48px; margin-bottom: 10px;'>✓</div>
            <h1 style='margin: 0; font-size: 28px;'>Payment Successful!</h1>
        </div>
        
        <div class='content'>
            <p style='font-size: 16px;'>Dear <strong>{booking.User.FullName}</strong>,</p>
            
            <p>Your payment has been processed successfully. Your booking is now fully confirmed!</p>
            
            <div class='payment-box'>
                <div class='detail-row'>
                    <span style='color: #065f46; font-weight: 500;'>Transaction ID:</span>
                    <span style='color: #065f46; font-weight: 600;'>{payment.TransactionId}</span>
                </div>
                <div class='detail-row'>
                    <span style='color: #065f46; font-weight: 500;'>Amount Paid:</span>
                    <span class='highlight'>LKR {payment.Amount:N2}</span>
                </div>
                <div class='detail-row'>
                    <span style='color: #065f46; font-weight: 500;'>Payment Method:</span>
                    <span style='color: #065f46; font-weight: 600;'>{payment.PaymentMethod}</span>
                </div>
                <div class='detail-row'>
                    <span style='color: #065f46; font-weight: 500;'>Booking Reference:</span>
                    <span style='color: #065f46; font-weight: 600;'>{booking.BookingReference}</span>
                </div>
                <div class='detail-row' style='border-bottom: none;'>
                    <span style='color: #065f46; font-weight: 500;'>Payment Date:</span>
                    <span style='color: #065f46; font-weight: 600;'>{payment.PaymentDate:MMMM dd, yyyy HH:mm}</span>
                </div>
            </div>
            
            <div style='background-color: #eff6ff; padding: 20px; border-radius: 8px; margin: 20px 0;'>
                <p style='margin: 0; color: #1e40af; font-weight: bold;'>📧 What's Next?</p>
                <ul style='color: #1e40af; margin: 10px 0; padding-left: 20px;'>
                    <li>You will receive a separate booking confirmation email</li>
                    <li>Keep this email for your records</li>
                    <li>Present your booking reference at check-in</li>
                </ul>
            </div>
            
            <p style='font-size: 16px;'>Thank you for your payment!</p>
        </div>
        
        <div class='footer'>
            <p><strong style='color: #10b981;'>Hotel Service</strong> - Your Trusted Booking Partner</p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(booking.User.Email, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                

                using var smtpClient = new SmtpClient(emailSettings.SmtpHost, emailSettings.SmtpPort)
                {
                    Credentials = new NetworkCredential(emailSettings.SmtpUser, emailSettings.SmtpPassword),
                    EnableSsl = emailSettings.EnableSsl,
                    Timeout = 30000
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailSettings.FromEmail, emailSettings.FromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    Priority = MailPriority.High
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }
    }
}

        
