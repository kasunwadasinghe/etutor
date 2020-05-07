using ETutor.ViewModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace ETutor.Common.Mail
{
    public class Mail
    {
        public static void SendMail(string to,string from,string subject,string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                //SmtpClient SmtpServer = new SmtpClient();
                //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //SmtpServer.Port = 25;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("greenwitchetutor@gmail.com", "Etutor@123");
                //SmtpServer.EnableSsl = false;
                //SmtpServer.UseDefaultCredentials = false;

                SmtpClient SmtpServer = new SmtpClient("webmail.wunderkindit.com");
                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("info@wunderkindit.com", "!nfo@123");
                SmtpServer.EnableSsl = false;
                SmtpServer.UseDefaultCredentials = false;

                mail.From = new MailAddress("info@wunderkindit.com");
                mail.To.Add(to);
                mail.To.Add(from);
                mail.Subject = subject;

                mail.IsBodyHtml = true;
                string htmlBody;
                htmlBody = string.Format("<html>< body >< h1 > ETutor System </ h1 ><hr/>< p > Hi {0},</ p >< p > {1} </ P ></ body ></ html >", to, body);

                switch (subject)
                {
                    case "Cancel meeting":
                        {
                            htmlBody = string.Format("<html>< body >< h1 > ETutor System </ h1 ><hr/>< p > Hi {0},</ p >< p > Your meeting is canceled </ P ></ body ></ html >", to);
                            break;
                        }
                    default: 
                        {
                            htmlBody = string.Format("<html>< body >< h1 > ETutor System </ h1 ><hr/>< p > Hi {0},</ p >< p > You have a new message </ P ></ body ></ html >", to);
                            break;
                        }
                }

                mail.Body = htmlBody;

                

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Logger.log(ex.Source, ex.Message);
                Console.WriteLine(ex.ToString());
            }
        }
    }
}