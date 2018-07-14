using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Security;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace WTO.Handler
{
    public enum MailType
    {
        smtp_CII = 1,
        smtp_Teamup = 2,
        smtp_WTO = 3
    }
    public sealed class SendMail
    {
        /// <summary>
        /// Send mails by choosing mail id predefined in web.config file.
        /// </summary>
        /// <param name="MailType">Define number of mails which can be used to send mails by Mailer.SendMail() function.</param>
        /// <param name="To">Mail will be send to email address provided in this parameter. For multiple emails use ';' as seprator.</param>
        /// <param name="CC">A CC will be send to email address provided in this parameter. For multiple emails use ';' as seprator.</param>
        /// <param name="BCC">A BCC will be send to email address provided in this parameter. For multiple emails use ';' as seprator.</param>
        /// <param name="ReplyTo">Provide an email address on which you want to get reply.</param>
        /// <param name="DisplayName"></param>
        /// <param name="Subject">Subject of the mail.</param>
        /// <param name="Body">The message that you want to send. It can contains HTML markups.</param>
        /// <param name="Attachments">Attachments with mails. To send multiple use an array of string.</param>
        /// <returns>Returns a boolean value to indicate wheather mail has been sent successfuly or not.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static Task SendEMail(string To, string CC, string BCC, string ReplyTo, string DisplayName, string Subject, string Body, string[] Attachments)
        {
            if (String.IsNullOrEmpty(To))
                throw new ArgumentNullException("To cannot be blank");
            if (String.IsNullOrEmpty(Subject))
                throw new ArgumentNullException("Subject cannot be blank.");

            string[] xToSplit = To.Split(';');
            string[] xCCSplit = CC.Split(';');
            string[] xBCCSplit = BCC.Split(';');

            MailMessage xmail = new MailMessage();

            if (DisplayName.Trim().Length > 0)
                xmail.From = new System.Net.Mail.MailAddress("sps-tbtcomm@gov.in", DisplayName.Trim());
            else
                xmail.From = new System.Net.Mail.MailAddress("sps-tbtcomm@gov.in");

            for (int indx = 0; indx < xToSplit.Length; indx++)
                if (xToSplit[indx].Trim().Length > 0)
                    xmail.To.Add(xToSplit[indx]);

            for (int indx = 0; indx < xCCSplit.Length; indx++)
                if (xCCSplit[indx].Trim().Length > 0)
                    xmail.CC.Add(xCCSplit[indx]);

            for (int indx = 0; indx < xBCCSplit.Length; indx++)
                if (xBCCSplit[indx].Trim().Length > 0)
                    xmail.Bcc.Add(xBCCSplit[indx]);

            if (ReplyTo.Trim().Length > 0)
                xmail.ReplyToList.Add(new System.Net.Mail.MailAddress(ReplyTo.Trim(), ""));

            xmail.Subject = Subject;
            xmail.Body = Body;
            xmail.IsBodyHtml = true;

            try
            {
                if (Attachments.Length > 0)
                {
                    for (int i = 0; i < Attachments.Length; i++)
                    {
                        Attachment at = new Attachment(Attachments[i].ToString());
                        xmail.Attachments.Add(at);
                    }
                }
            }
            catch { }

            SmtpClient xsmtp = new SmtpClient();
            xsmtp.Host = "smtp.gmail.com";
            xsmtp.Port = 587;
            xsmtp.EnableSsl = true;
            xsmtp.UseDefaultCredentials = true;
            xsmtp.Credentials = new NetworkCredential("sps-tbtcomm@gov.in", "C1%uM2@iY1");
            xsmtp.Timeout = 1500000;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            try
            {
                return xsmtp.SendMailAsync(xmail);
            }
            catch (Exception ex) { }
            return null;
        }

        //Send mail async
        delegate bool SendMailAsyncDelegate(Enum MailType, string To, string CC, string BCC, string ReplyTo, string DisplayName, string Subject, string Body, string[] Attachments);

        public void SendAsyncEMail(Enum MailType, string To, string CC, string BCC, string ReplyTo, string DisplayName, string Subject, string Body, string[] Attachments)
        {
            SendMailAsyncDelegate dc = new SendMailAsyncDelegate(SendMailAsync);
            AsyncCallback cb = new AsyncCallback(GetResultsOnCallback);
            IAsyncResult ar = dc.BeginInvoke(MailType, To, CC, BCC, ReplyTo, DisplayName, Subject, Body, Attachments, cb, null);
        }

        void GetResultsOnCallback(IAsyncResult ar)
        {
            SendMailAsyncDelegate del = (SendMailAsyncDelegate)((AsyncResult)ar).AsyncDelegate;
            try
            {
                del.EndInvoke(ar);
            }
            catch (Exception ex) { }
        }

        bool SendMailAsync(Enum MailType, string To, string CC, string BCC, string ReplyTo, string DisplayName, string Subject, string Body, string[] Attachments)
        {
            try
            {
                if (String.IsNullOrEmpty(To))
                    throw new ArgumentNullException("To cannot be blank");
                if (String.IsNullOrEmpty(Subject))
                    throw new ArgumentNullException("Subject cannot be blank.");

                string[] xToSplit = To.Split(';');
                string[] xCCSplit = CC.Split(';');
                string[] xBCCSplit = BCC.Split(';');

                MailMessage xmail = new MailMessage();

                SmtpSection smtp = (SmtpSection)ConfigurationManager.GetSection("mailSettings/" + MailType.ToString());

                if (DisplayName.Trim().Length > 0)
                    xmail.From = new System.Net.Mail.MailAddress(smtp.Network.UserName, DisplayName.Trim());
                else
                    xmail.From = new System.Net.Mail.MailAddress(smtp.Network.UserName);

                for (int indx = 0; indx < xToSplit.Length; indx++)
                    if (xToSplit[indx].Trim().Length > 0)
                        xmail.To.Add(xToSplit[indx]);

                for (int indx = 0; indx < xCCSplit.Length; indx++)
                    if (xCCSplit[indx].Trim().Length > 0)
                        xmail.CC.Add(xCCSplit[indx]);

                for (int indx = 0; indx < xBCCSplit.Length; indx++)
                    if (xBCCSplit[indx].Trim().Length > 0)
                        xmail.Bcc.Add(xBCCSplit[indx]);

                if (ReplyTo.Trim().Length > 0)
                    xmail.ReplyToList.Add(new System.Net.Mail.MailAddress(ReplyTo.Trim(), ""));

                xmail.Subject = Subject;
                xmail.Body = Body;
                xmail.IsBodyHtml = true;

                try
                {
                    if (Attachments.Length > 0)
                    {
                        for (int i = 0; i < Attachments.Length; i++)
                        {
                            Attachment at = new Attachment(Attachments[i].ToString());
                            xmail.Attachments.Add(at);
                        }
                    }
                }
                catch { }

                SmtpClient xsmtp = new SmtpClient();
                xsmtp.Host = smtp.Network.Host;
                xsmtp.Port = smtp.Network.Port;
                xsmtp.EnableSsl = true;
                xsmtp.UseDefaultCredentials = smtp.Network.DefaultCredentials;
                xsmtp.Credentials = new NetworkCredential(smtp.Network.UserName, smtp.Network.Password);
                xsmtp.Timeout = 1500000;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                xsmtp.Send(xmail);
                return true;
            }
            catch (Exception ex)
            {
                File.AppendAllText(Environment.CurrentDirectory + "\\XceptionLog.txt",
                        String.Format(Environment.NewLine + "[{0}] - {1}, {2}, {3} , {4}",
                        DateTime.Now.ToString("dd MMM yyyy HH:mm:ss"), "", "", ex.Message, ex.StackTrace));

                return false;
            }
        }
        //bool SendMailAsync(string To, string CC, string BCC, string ReplyTo, string DisplayName, string Subject, string Body, string[] Attachments)
        //{
        //    try
        //    {
        //        if (String.IsNullOrEmpty(To))
        //            throw new ArgumentNullException("To cannot be blank");
        //        if (String.IsNullOrEmpty(Subject))
        //            throw new ArgumentNullException("Subject cannot be blank.");

        //        string[] xToSplit = To.Split(';');
        //        string[] xCCSplit = CC.Split(';');
        //        string[] xBCCSplit = BCC.Split(';');

        //        MailMessage xmail = new MailMessage();

        //        SmtpSection smtp = (SmtpSection)ConfigurationManager.GetSection("mailSettings/");

        //        //if (DisplayName.Trim().Length > 0)
        //        //    xmail.From = new System.Net.Mail.MailAddress("sps-tbtcomm@gov.in", DisplayName.Trim());
        //        //else
        //        //    xmail.From = new System.Net.Mail.MailAddress("sps-tbtcomm@gov.in");

        //        if (DisplayName.Trim().Length > 0)
        //            xmail.From = new System.Net.Mail.MailAddress("mishra.ashvini@chipsoftindia.com", DisplayName.Trim());
        //        else
        //            xmail.From = new System.Net.Mail.MailAddress("mishra.ashvini@chipsoftindia.com");

        //        for (int indx = 0; indx < xToSplit.Length; indx++)
        //            if (xToSplit[indx].Trim().Length > 0)
        //                xmail.To.Add(xToSplit[indx]);

        //        for (int indx = 0; indx < xCCSplit.Length; indx++)
        //            if (xCCSplit[indx].Trim().Length > 0)
        //                xmail.CC.Add(xCCSplit[indx]);

        //        for (int indx = 0; indx < xBCCSplit.Length; indx++)
        //            if (xBCCSplit[indx].Trim().Length > 0)
        //                xmail.Bcc.Add(xBCCSplit[indx]);

        //        if (ReplyTo.Trim().Length > 0)
        //            xmail.ReplyToList.Add(new System.Net.Mail.MailAddress(ReplyTo.Trim(), ""));

        //        xmail.Subject = Subject;
        //        xmail.Body = Body;
        //        xmail.IsBodyHtml = true;

        //        try
        //        {
        //            if (Attachments.Length > 0)
        //            {
        //                for (int i = 0; i < Attachments.Length; i++)
        //                {
        //                    Attachment at = new Attachment(Attachments[i].ToString());
        //                    xmail.Attachments.Add(at);
        //                }
        //            }
        //        }
        //        catch { }

        //        SmtpClient xsmtp = new SmtpClient();
        //        xsmtp.Host = smtp.Network.Host;
        //        xsmtp.Port = smtp.Network.Port;
        //        xsmtp.EnableSsl = true;
        //        xsmtp.UseDefaultCredentials = smtp.Network.DefaultCredentials;
        //        xsmtp.Credentials = new NetworkCredential("mishra.ashvini@chipsoftindia.com", "Aaditya@21");
        //        xsmtp.Timeout = 1500000;
        //        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

        //        xsmtp.Send(xmail);
        //        return true;


        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
    }
}