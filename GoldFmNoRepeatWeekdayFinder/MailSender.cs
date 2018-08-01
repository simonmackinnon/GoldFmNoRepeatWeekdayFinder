using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;

namespace GoldFmNoRepeatWeekdayFinder
{
    class MailSender
    {
        private MailAddress fromAddress;
        private SecureString fromPassword;
        private MailAddress toAddress;

        public MailSender(string address, SecureString password, string to)
        {
            fromAddress = new MailAddress(address, "Song Alert");
            fromPassword = password;
            toAddress = new MailAddress(to, "Simon Mackinnon");
        }

        public void sendStartEmail()
        {

            string subject = "Song analyser alert: Starting";
            string body = "Song analyser alert: Starting @" + DateTime.Now.ToString();

            string password = new System.Net.NetworkCredential(string.Empty, fromPassword).Password;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, password)
            };

            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error seding start email: " + e);
            }
            
        }

        public void sendEmailAlert(Song song, Playlist playlist)
        {

            string subject = "Song analyser alert: " + song.title + " - " + song.artist;
            string body = "the song " + song.title + " - " + song.artist + " has been played multiple times today.\n";

            foreach (var song_local in playlist.playlist.Where(x => x.artist == song.artist && x.title == song.title))
            {
                body += "Time: " + song_local.playedDateTime + "\n";
            }

            body += "Call (03) 9414 1043";

            string password = new System.Net.NetworkCredential(string.Empty, fromPassword).Password;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, password)
           };


            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error seding alert email: " + e);
            }
        }
    }
}
