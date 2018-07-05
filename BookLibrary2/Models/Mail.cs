using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace BookLibrary2.Models
{
    public class Mail : IDisposable
    {

        private const String Host = "feedback-smtp.eu-west-1.amazonses.com";

        // The port you will connect to on the Amazon SES SMTP endpoint. We
        // are choosing port 587 because we will use STARTTLS to encrypt
        // the connection.
        private const int Port = 587;

        // Replace sender@example.com with your "From" address. 
        // This address must be verified with Amazon SES.        
        private const String Fromname = "Book Library";

        // Replace smtp_username with your Amazon SES SMTP user name.
        private const String SmtpUsername = "AKIAJIMW4QHGRDGKQUBQ";

        // Replace smtp_password with your Amazon SES SMTP user name.
        private const String SmtpPassword = "Apxks204HYZJfYfZutsj31pGMt4tWuDSM2cZsDOwX0mq";

       

        // If you're using Amazon SES in a region other than US West (Oregon), 
        // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
        // endpoint in the appropriate Region.
        public Mail()
        {
            // Pass SMTP credentials
            // Enable SSL encryption
            _client = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(SmtpUsername, SmtpPassword),
                EnableSsl = true
            };            
        }

        public bool SendGmail(string subject, string content, string recipient, string from)
        {

            // (Optional) the name of a configuration set to use for this message.
            // If you comment out this line, you also need to remove or comment out
            // the "X-SES-CONFIGURATION-SET" header below.
            const String configset = "ConfigSet";

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(from, Fromname);
            message.To.Add(new MailAddress(recipient));
            message.Subject = subject;
            message.Body = content;
            // Comment or delete the next line if you are not using a configuration set
            message.Headers.Add("X-SES-CONFIGURATION-SET", configset);

            // Create and configure a new SmtpClient
           
           

            // Send the email. 
            try
            {
                _client.Send(message);
               
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                message.Dispose();
            }
        
            return true;
        }

        public void Dispose()
        {
            if (_client != null) _client.Dispose();
        }

        private readonly SmtpClient _client;      
    }
}