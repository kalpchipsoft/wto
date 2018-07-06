using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BusinessObjects.Notification;
using BusinessService.Notification;
using WTO.Handler;
using System.IO;
using System.Web;
using BusinessObjects.ManageAccess;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;
using iTextSharp.text;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WTO.Controllers.API.WTO
{
    public class AddUpdateNotificationController : ApiController
    {
        [HttpPost]
        public IHttpActionResult ReadNotificationDetails(BusinessObjects.Notification.Attachment file)
        {
            NotificationDetails objE = new NotificationDetails();
            try
            {
                if (file != null && file.Content.Length > 0)
                {
                    #region "Get & save Document"
                    byte[] bytes = null;
                    if (file.Content.IndexOf(',') >= 0)
                    {
                        var myString = file.Content.Split(new char[] { ',' });
                        bytes = Convert.FromBase64String(myString[1]);
                    }
                    else
                        bytes = Convert.FromBase64String(file.Content);

                    string filePath = HttpContext.Current.Server.MapPath("/Attachments/Temp/" + file.FileName);

                    if (file.FileName.Length > 0 && bytes.Length > 0)
                    {
                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        File.WriteAllBytes(filePath, bytes);
                    }

                    //Stream st = new MemoryStream(bytes);
                    string _fileName = System.IO.Path.GetFileName(file.FileName);
                    string fileExtension = System.IO.Path.GetExtension(_fileName);
                    #endregion

                    #region "Pdf File"
                    if (fileExtension.ToLower() == ".pdf")
                    {
                        #region "REad PDF"
                        //PdfReader reader = new PdfReader(filePath);
                        //int intPageNum = reader.NumberOfPages;
                        //string[] words;
                        //string line;

                        //for (int i = 1; i <= intPageNum; i++)
                        //{

                        //    string text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());
                        //    List<string> Data = new List<string>();
                        //    if (text.Trim().Contains("\a"))
                        //    {
                        //        foreach (string str in text.Trim().Replace("\a", "#").Split('#'))
                        //        {
                        //            if (str.Trim() != "")
                        //                Data.Add(str);
                        //        }
                        //    }
                        //    else if (text.Trim().Contains("\n"))
                        //    {
                        //        foreach (string str in text.Trim().Replace("\n", "").Replace("\r", "#").Split('#'))
                        //        {
                        //            if (str.Trim() != "")
                        //                Data.Add(str);
                        //        }
                        //    }

                        //    words = text.Split('\n');
                        //    for (int j = 0, len = words.Length; j < len; j++)
                        //    {
                        //        line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));
                        //    }

                        //    #region "Loop start"
                        //    foreach (string str in Data)
                        //    {
                        //        string Header = str;
                        //        Header = Regex.Replace(Header, @"[^\w\s.,!/@#$%^&*()=+~`-]", "");
                        //        Header = Regex.Replace(Header, @"\r\r+", ",");
                        //        if (Header.Split(',').Length > 1)
                        //        {
                        //            foreach (string hdrstr in Header.Split(','))
                        //            {
                        //                string Ctext = hdrstr;

                        //                if (Ctext.Contains("G/TBT/N/"))
                        //                {
                        //                    objE.NotificationNumber = Ctext;
                        //                    objE.Type = "2";
                        //                }
                        //                else if (Ctext.Contains("G/SPS/N/"))
                        //                {
                        //                    objE.NotificationNumber = Ctext;
                        //                    objE.Type = "1";
                        //                }

                        //                DateTime Temp;
                        //                if (DateTime.TryParse(Ctext, out Temp) == true)
                        //                {
                        //                    DateTime d = Convert.ToDateTime(DateTime.Parse(Ctext.Trim()).ToString("dd MM yyyy", System.Globalization.CultureInfo.InvariantCulture));
                        //                    objE.NotificationDate = d.ToString("dd MMM yyyy");
                        //                }
                        //            }
                        //        }

                        //        if (str.Contains("/SPS/") || str.Contains("/TBT/"))
                        //        {
                        //            objE.NotificationNumber = Regex.Replace(str, @"[^\w\s.,!/@#$%^&*()=+~`]", "#").Replace("\r", "").Split('#')[0];

                        //            if (objE.NotificationNumber.Length > 15)
                        //            {
                        //                objE.NotificationNumber = objE.NotificationNumber.Substring(0, 15);
                        //                objE.NotificationDate = objE.NotificationNumber.Substring(15, 13);
                        //            }

                        //            if (objE.NotificationNumber.Contains("/SPS/"))
                        //                objE.Type = "1";
                        //            else if (objE.NotificationNumber.Contains("/TBT/"))
                        //                objE.Type = "2";

                        //        }

                        //        if (str.Contains("Notified under Article"))
                        //        {
                        //            objE.Articles = "";
                        //            foreach (string ar in str.Replace("Notified under Article ", "").Split(','))
                        //            {
                        //                if (ar.Contains("["))
                        //                {
                        //                    if ((ar.Split('[')[1]).Contains("X"))
                        //                        objE.Articles += ar.Split('[')[0].Trim() + ",";
                        //                }
                        //            }
                        //        }


                        //        if (str.Contains("Notifying Member"))
                        //        {
                        //            string NotifyingMember = str.Replace("Notifying Member:", "");
                        //            objE.Country = Regex.Replace(NotifyingMember, @"[^\w\s.,!@#$%^&*()=+~`-]", "#").Split('#')[0].Split('\r')[0];
                        //        }

                        //        if (str.Contains("Agency responsible"))
                        //        {
                        //            string AgencyResponsible = str.Replace("Agency responsible:", "");
                        //            objE.ResponsibleAgency = Regex.Replace(AgencyResponsible, @"[^\w\s.,!@#$%^&*()=+~`-]", "#").Split('#')[0].Split('\r')[0];
                        //        }

                        //        if (str.Contains("Products covered"))
                        //        {
                        //            string ProductsCovered = str.Split(':')[str.Split(':').Length - 1];
                        //            objE.ProductsCovered = Regex.Replace(ProductsCovered, @"[^\w\s.,!@#$%^&*()=+~`-]", "#").Split('#')[0].Split('\r')[0];
                        //        }

                        //        if (str.Contains("Title"))
                        //        {
                        //            if (str.Contains("Title of the notified document"))
                        //            {
                        //                string Title = str.Replace("Title of the notified document:", "");
                        //                objE.Title = Regex.Replace(Title, @"[^\w\s.,!@#$%^&*()=+~`-]", "#").Split('#')[0].Split('\r')[0];
                        //            }
                        //            else
                        //            {
                        //                string Title = str.Split(':')[str.Split(':').Length - 1];
                        //                objE.Title = Regex.Replace(Title, @"[^\w\s.,!@#$%^&*()=+~`-]", "#").Split('#')[0].Split('\r')[0];
                        //            }
                        //        }

                        //        if (str.Contains("Description of content"))
                        //        {
                        //            string Description = str.Replace("Description of content:", "");
                        //            objE.Description = Regex.Replace(Description, @"[^\w\s.,!@#$%^&*()=+~`-]", "#").Split('#')[0].Split('\r')[0];
                        //        }

                        //        if (str.Contains("Final date for comments"))
                        //        {
                        //            string FinalDateForComments = str.Split('\r')[0].Split(':')[str.Split('\r')[0].Split(':').Length - 1];
                        //            objE.FinalDateOfComments = Regex.Replace(FinalDateForComments, @"[^\w\s.,!@#$%^&*()=+~`-]", "#").Split('#')[0].Split('\r')[0];
                        //        }
                        //    }
                        //    #endregion
                        //}
                        #endregion
                    }
                    #endregion

                    #region "Word File"
                    if (fileExtension.ToLower() == ".doc" || fileExtension.ToLower() == ".docx")
                    {
                        //running
                        #region "Read file using Interop"
                        Application word = new Application(); ;

                        Microsoft.Office.Interop.Word.Document wordfile = new Microsoft.Office.Interop.Word.Document();
                        object path = filePath;
                        // Define an object to pass to the API for missing parameters
                        object missing = System.Type.Missing;
                        wordfile = word.Documents.Open(ref path,
                                ref missing, ref missing, ref missing, ref missing,
                                ref missing, ref missing, ref missing, ref missing,
                                ref missing, ref missing, ref missing, ref missing,
                                ref missing, ref missing, ref missing);

                        try
                        {
                            String read = string.Empty;
                            List<string> data = new List<string>();
                            List<string> headers = new List<string>();
                            bool IsMultipleNotificationNumber = false;
                            #region "Read File Header"
                            foreach (Microsoft.Office.Interop.Word.Section aSection in word.ActiveDocument.Sections)
                            {
                                foreach (HeaderFooter aHeader in aSection.Headers)
                                {
                                    headers.Add(aHeader.Range.Text);
                                    string Header = aHeader.Range.Text;
                                    Header = Regex.Replace(Header, @"[^\w\s.,!/@#$%^&*()=+~`-]", "");
                                    Header = Regex.Replace(Header, @"\r\r+", ",");
                                    if (Header.Split(',').Length > 1)
                                    {
                                        foreach (string hdrstr in Header.Split(','))
                                        {
                                            string Ctext = hdrstr;

                                            if (Ctext.Contains("G/TBT/N/"))
                                            {
                                                objE.NotificationNumber = Ctext;
                                                objE.NotificationType = "2";
                                            }
                                            else if (Ctext.Contains("G/SPS/N/"))
                                            {
                                                objE.NotificationNumber = Ctext;
                                                objE.NotificationType = "1";
                                            }

                                            DateTime Temp;
                                            if (DateTime.TryParse(Ctext, out Temp) == true)
                                                objE.DateofNotification = Convert.ToDateTime(Ctext.Trim()).ToString("dd MMM yyyy");
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region "File Body"
                            if (objE.NotificationType == "1")
                            {
                                if (objE.NotificationNumber.IndexOf("SPS") != objE.NotificationNumber.LastIndexOf("SPS"))
                                    IsMultipleNotificationNumber = true;
                            }
                            else if (objE.NotificationType == "2")
                            {
                                if (objE.NotificationNumber.IndexOf("TBT") != objE.NotificationNumber.LastIndexOf("TBT"))
                                    IsMultipleNotificationNumber = true;
                            }

                            if (objE.NotificationNumber.Split('/').Length < 6 || IsMultipleNotificationNumber)
                            {
                                #region "Table"
                                Table t = wordfile.Tables[1];
                                Rows rows = t.Rows;
                                foreach (Row tablerow in rows)
                                {
                                    String str = tablerow.Range.Text;
                                    if (str.Contains("Notifying Member"))
                                    {
                                        foreach (string s in Regex.Replace(str, @"[^\w\s.,!@#$%^&*()=+~`-]", "").Trim().Split('\r'))
                                        {
                                            if (s.Contains("Notifying Member"))
                                                objE.Country = s.Replace("Notifying Member", "").Trim();
                                        }
                                    }

                                    if (str.Contains("Agency responsible"))
                                    {
                                        string _ResponsibleAgency = Regex.Replace(str, @"[^:\w\s.,!@#$%^&*()=+~`-]", "").Trim();
                                        objE.ResponsibleAgency = _ResponsibleAgency.Substring(_ResponsibleAgency.IndexOf(":", 0) + 1);
                                    }

                                    if (str.Contains("Products covered"))
                                    {
                                        string _Products = Regex.Replace(str, @"[^:\w\s.,!@#$%^&*()=+~`-]", "").Trim();
                                        int startIndex = _Products.IndexOf(":", 0);
                                        if (startIndex >= 0)
                                        {
                                            _Products = _Products.Substring(startIndex + 1);
                                            objE.ProductsCovered = _Products;

                                            string hs = "";
                                            if (_Products.Contains("(HS "))
                                            {
                                                hs = _Products.Remove(0, _Products.IndexOf("(HS"));
                                                hs = hs.Substring(0, hs.IndexOf(')'));
                                                hs = hs.Replace("(HS", "").Trim();
                                                objE.HSCodes = hs;
                                            }
                                            else if (_Products.Contains("(HS:"))
                                            {
                                                hs = _Products.Substring(_Products.IndexOf("(HS:") + 1).Replace(")", "");
                                                hs = hs.Substring(0, hs.IndexOf(","));
                                                hs = hs.Replace("HS:", "").Trim();
                                                objE.HSCodes = hs.Replace('.', ',');
                                            }
                                        }
                                    }

                                    if (str.Contains("Title"))
                                    {
                                        string _Title = Regex.Replace(str, @"[^:\w\s.,!@#$%^&*()=+~`-]", "").Trim();
                                        if (objE.NotificationNumber.Contains("/SPS/") && _Title.Contains("Title of the notified document:"))
                                            objE.Title = _Title.Replace("Title of the notified document:", "").Trim();
                                        else if (objE.NotificationNumber.Contains("/TBT/") && _Title.Contains("Title, number of pages and language(s) of the notified document:"))
                                            objE.Title = _Title.Replace("Title, number of pages and language(s) of the notified document:", "").Trim();

                                        objE.Title = objE.Title.Replace("5.", "");
                                    }

                                    if (str.Contains("Description of content"))
                                    {
                                        foreach (string s in Regex.Replace(str, @"[^\w\s.,!@#$%^&*()=+~`-]", "").Trim().Split('\r'))
                                        {
                                            if (s.Contains("Description of content"))
                                                objE.Description = s.Replace("Description of content", "").Trim();
                                        }
                                        var _Desc = Regex.Replace(str, @"[^\w\s.,!@#$%^&*()=+~`-]", "").Trim();
                                        objE.Description = _Desc.Replace("Description of content", "").Trim();
                                        objE.Description = objE.Description.Replace("6.", "");
                                    }

                                    if (str.Contains("Final date for comments"))
                                    {
                                        foreach (string s in Regex.Replace(str, @"[^:\w\s.,!@#$%^&*()=+~`-]", "").Trim().Split('\r'))
                                        {
                                            if (s.Contains("Final date for comments"))
                                            {
                                                string FinalDateForComments = s.Split(':')[s.Split(':').Length - 1].Trim();
                                                DateTime Temp;
                                                if (DateTime.TryParse(FinalDateForComments, out Temp) == true)
                                                    objE.FinalDateOfComments = FinalDateForComments.Trim();
                                            }
                                        }
                                    }

                                    if (str.Contains("Notified under Article"))
                                    {
                                        foreach (string s in Regex.Replace(str, @"[^:\w\s.,!@#$%^&*()=+~`-]", "").Trim().Split('\r'))
                                        {
                                            if (s.Contains("Notified under Article"))
                                            {
                                                foreach (string ar in s.Replace("Notified under Article ", "").Split(','))
                                                {
                                                    if (ar.Contains("X"))
                                                        objE.Articles += ar.Replace("X", "").Trim() + ",";
                                                }
                                            }
                                        }
                                    }

                                    if (str.Contains("Texts available from") || str.Contains("Text(s) available from"))
                                    {
                                        foreach (string s in Regex.Replace(str, @"[^:\w\s.,!@#$%^&*()=+~`-]", "").Trim().Split('\r'))
                                        {
                                            if (s.Contains("Email"))
                                            {
                                                string email = s.Replace("E-mail", "").Replace("Email", "").Replace(":", "").Trim();
                                                if (Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                                                    objE.EnquiryEmailId = email;
                                            }
                                            else if (s.Contains("E-mail"))
                                            {
                                                string email = s.Substring(s.IndexOf("E-mail")).Replace("E-mail:", "").Trim();
                                                if (email.IndexOf("\v") > 0)
                                                    email = email.Substring(0, email.IndexOf("\v"));
                                                if (email.IndexOf(',') >= 0)
                                                {
                                                    var _EnquiryEmailId = "";
                                                    foreach (string e in email.Split(','))
                                                    {
                                                        if (Regex.IsMatch(e, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                                                            _EnquiryEmailId += e + ";";
                                                    }
                                                    objE.EnquiryEmailId = _EnquiryEmailId.TrimEnd(';');
                                                }
                                                else
                                                {
                                                    if (Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                                                        objE.EnquiryEmailId = email;
                                                }
                                            }
                                        }
                                    }
                                }

                                Marshal.ReleaseComObject(t);
                                Marshal.ReleaseComObject(rows);
                                Marshal.ReleaseComObject(wordfile);
                                #endregion
                            }

                            if (IsMultipleNotificationNumber)
                                objE.NotificationNumber = "";
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            File.AppendAllText(HttpContext.Current.Server.MapPath("~/XceptionLog.txt"), ex.Message);

                            ((_Document)wordfile).Close();
                            if (word.Documents.Count > 0)
                                word.Documents.Close();

                            ((_Application)word.Application).Quit();
                            word.Quit();

                            if (File.Exists(filePath))
                                File.Delete(filePath);
                        }
                        finally
                        {

                            ((_Document)wordfile).Close();
                            if (word.Documents.Count > 0)
                                word.Documents.Close();

                            ((_Application)word.Application).Quit();
                            word.Quit();

                            if (File.Exists(filePath))
                                File.Delete(filePath);
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(HttpContext.Current.Server.MapPath("~/XceptionLog.txt"), ex.Message);
            }

            if (objE != null && objE.NotificationNumber != null)
            {
                if (objE.NotificationNumber.Split('/').Length < 6)
                {
                    CheckNotification objI = new CheckNotification();
                    objI.NotificationNumber = objE.NotificationNumber;
                    objI.DateOfNotification = objE.DateofNotification;
                    if (!string.IsNullOrEmpty(objE.FinalDateOfComments))
                        objI.FinalDateOfComment = objE.FinalDateOfComments;

                    NotificationBusinessService objAN = new NotificationBusinessService();
                    ValidateNotification_Output objO = new ValidateNotification_Output();
                    objO = objAN.ValidateNotification(objI);

                    if (objO != null)
                    {
                        objE.SendResponseBy = objO.SendResponseBy;
                        objE.FinalDateOfComments = objO.FinalDateofComment;
                        objE.StakeholderResponseDueBy = objO.StakeholderResponseBy;
                    }
                }
            }
            return Ok(objE);
        }

        [HttpPost]
        public IHttpActionResult SendMailToEnquiryDesk(long Id, SendMailToEnquiryDesk MailDetails)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            SendMail_Output objOutput = objAN.SaveAndSendMailToEnquiryDesk(Id, MailDetails);

            if (objOutput != null)
            {
                SendMail objMail = new SendMail();
                objOutput.MailDetails.Body = objOutput.MailDetails.Body;
                objMail.SendAsyncEMail(objOutput.MailTo, objOutput.CC, objOutput.BCC, objOutput.ReplyTo, objOutput.DisplayName, objOutput.MailDetails.Subject, objOutput.MailDetails.Body, null);
            }

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult InsertUpdate_Notification(AddNotification obj)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            AddNoti_Result objOutput = objAN.InsertUpdateNotification(obj);
            if (objOutput.Status.ToLower() == "success")
            {
                #region "Attachments"

                #region "Notification Attachment"
                if (obj.NotificationAttachment != null && obj.NotificationAttachment.Content != "")
                {
                    try
                    {
                        byte[] bytes = null;
                        if (obj.NotificationAttachment.Content.IndexOf(',') >= 0)
                        {
                            var myString = obj.NotificationAttachment.Content.Split(new char[] { ',' });
                            bytes = Convert.FromBase64String(myString[1]);
                        }
                        else
                            bytes = Convert.FromBase64String(obj.NotificationAttachment.Content);

                        if (obj.NotificationAttachment.FileName.Length > 0 && bytes.Length > 0)
                        {
                            string filePath = HttpContext.Current.Server.MapPath("/Attachments/NotificationAttachment/" + objOutput.NotificationId + "_" + obj.NotificationAttachment.FileName);
                            File.WriteAllBytes(filePath, bytes);
                        }
                    }
                    catch (Exception ex) { }
                }
                #endregion

                #region "Full text(s) of regulation"
                if (obj.NotificationDoc != null && obj.NotificationDoc.Count > 0)
                {
                    foreach (BusinessObjects.Notification.Attachment objAttach in obj.NotificationDoc)
                    {
                        foreach (BusinessObjects.Notification.EditAttachment Att in objOutput.Attachments)
                        {
                            if (Att.FileName == objAttach.FileName && Att.Path.IndexOf("NotificationDocument") > 0)
                            {
                                try
                                {
                                    byte[] bytes = null;
                                    if (objAttach.Content.IndexOf(',') >= 0)
                                    {
                                        var myString = objAttach.Content.Split(new char[] { ',' });
                                        bytes = Convert.FromBase64String(myString[1]);
                                    }
                                    else
                                        bytes = Convert.FromBase64String(objAttach.Content);

                                    if (objAttach.FileName.Length > 0 && bytes.Length > 0)
                                    {
                                        string filePath = HttpContext.Current.Server.MapPath(Att.Path);
                                        File.WriteAllBytes(filePath, bytes);
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                }
                #endregion

                #region "Translated Documents"
                if (obj.TranslatedDoc != null && obj.TranslatedDoc.Count > 0)
                {
                    foreach (BusinessObjects.Notification.Attachment_TranslatedDoc objAttach in obj.TranslatedDoc)
                    {
                        foreach (BusinessObjects.Notification.EditAttachment Att in objOutput.Attachments)
                        {
                            if (Att.FileName == objAttach.FileName && Att.Path.IndexOf("NotificationDocument_Translated") > 0)
                            {
                                try
                                {
                                    byte[] bytes = null;
                                    if (objAttach.Content.IndexOf(',') >= 0)
                                    {
                                        var myString = objAttach.Content.Split(new char[] { ',' });
                                        bytes = Convert.FromBase64String(myString[1]);
                                    }
                                    else
                                        bytes = Convert.FromBase64String(objAttach.Content);

                                    if (objAttach.FileName.Length > 0 && bytes.Length > 0)
                                    {
                                        string filePath = HttpContext.Current.Server.MapPath(Att.Path);
                                        File.WriteAllBytes(filePath, bytes);
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                }
                #endregion
                #endregion
            }
            return Ok(objOutput);
        }

        [HttpPost]
        public IHttpActionResult SendToTranslater(SendToTranslater obj)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            SendToTranslater_Output objOutput = objAN.SendDocumentToTranslater(obj);

            if (objOutput != null && objOutput.TranslatorId > 0)
            {

                #region "Attachments"
                if (obj.Attachments != null && obj.Attachments.Count > 0 && objOutput.Attachments != null)
                {
                    foreach (TranslatorMailAttachment objAttach in obj.Attachments)
                    {
                        foreach (EditAttachment att in objOutput.Attachments)
                        {
                            if (objAttach.Content != "" && att.FileName == objAttach.FileName)
                            {
                                try
                                {
                                    byte[] bytes = null;
                                    if (objAttach.Content.IndexOf(',') >= 0)
                                    {
                                        var myString = objAttach.Content.Split(new char[] { ',' });
                                        bytes = Convert.FromBase64String(myString[1]);
                                    }
                                    else
                                        bytes = Convert.FromBase64String(objAttach.Content);

                                    if (objAttach.FileName.Length > 0 && bytes.Length > 0)
                                    {
                                        string filePath = HttpContext.Current.Server.MapPath(att.Path);
                                        File.WriteAllBytes(filePath, bytes);
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                }
                #endregion

                SendMail objMail = new SendMail();
                List<string> Attachment = new List<string>();
                foreach (EditAttachment objAttach in objOutput.Attachments)
                {
                    foreach (TranslatorMailAttachment objAt in obj.Attachments)
                    {
                        if (objAt.FileName == objOutput.FirstName)
                            Attachment.Add(HttpContext.Current.Server.MapPath(objAttach.Path));
                    }
                }

                objMail.SendAsyncEMail(objOutput.TranslaterEmailId, "", "atul.chipsoft@gmail.com", "", "Department of commerce", obj.MailDetails.Subject, obj.MailDetails.Message, Attachment.ToArray());
                return Ok(objOutput);
            }
            else
                return Content(HttpStatusCode.BadRequest, "Something Went wrong");
        }

        [HttpPost]
        public IHttpActionResult AddRelatedStackholders(AddStakeholder obj)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            return Ok(objAN.InsertDeleteRelatedStakeholders(obj.NotificationId, obj.StakeholderIds, false));
        }

        [HttpPost]
        public IHttpActionResult RemoveRelatedStackholders(AddStakeholder obj)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            return Ok(objAN.InsertDeleteRelatedStakeholders(obj.NotificationId, obj.StakeholderIds, true));
        }

        [HttpPost]
        public IHttpActionResult SendMailToStakeholders(SendMailStakeholders obj)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            SendMailStakeholders_Output objOutput = objAN.SaveAndSendMailToStakeholders(obj);

            if (objOutput != null)
            {
                #region "Attachments"
                if (obj.Attachments != null && obj.Attachments.Count > 0 && objOutput.Attachments != null)
                {
                    foreach (MailAttachment objAttach in obj.Attachments)
                    {
                        foreach (EditAttachment att in objOutput.Attachments)
                        {
                            if (objAttach.Content != "" && att.FileName == objAttach.FileName)
                            {
                                try
                                {
                                    byte[] bytes = null;
                                    if (objAttach.Content.IndexOf(',') >= 0)
                                    {
                                        var myString = objAttach.Content.Split(new char[] { ',' });
                                        bytes = Convert.FromBase64String(myString[1]);
                                    }
                                    else
                                        bytes = Convert.FromBase64String(objAttach.Content);

                                    if (objAttach.FileName.Length > 0 && bytes.Length > 0)
                                    {
                                        string filePath = HttpContext.Current.Server.MapPath(att.Path);
                                        File.WriteAllBytes(filePath, bytes);
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                }
                #endregion

                SendMail objMail = new SendMail();

                if (objOutput.StakeHolders != null)
                {
                    foreach (StakeHolder s in objOutput.StakeHolders)
                    {
                        string MailBody = objAN.MailbodyForStakeholders(objOutput.MailDetails);
                        MailBody = MailBody.Replace("#Stakeholder#", s.StakeHolderName);
                        List<string> Attachment = new List<string>();
                        foreach (MailAttachment att in obj.Attachments)
                        {
                            if (att.Path != "")
                                Attachment.Add(HttpContext.Current.Server.MapPath(att.Path));
                            else
                                Attachment.Add(HttpContext.Current.Server.MapPath("/Attachments/MailAttachment/" + objOutput.MailDetails.MailId + "_" + att.FileName));
                        }
                        objMail.SendAsyncEMail(s.Email, "", "atul.chipsoft@gmail.com", "", "Department of commerce", "WTO Notifications : Document for Translation", MailBody, Attachment.ToArray());
                    }
                }
            }

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult SaveStakeholderResponse(StakeholderResponse obj)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            SendMailStakeholders_Output objOutput = objAN.SaveStakeholderResponse(obj);

            if (objOutput != null)
            {
                #region "Response Attachments"
                if (obj.ResponseDocuments != null)
                {
                    foreach (ResponseAttachment objA in obj.ResponseDocuments)
                    {
                        try
                        {
                            byte[] bytes = null;
                            if (objA.Content.IndexOf(',') >= 0)
                            {
                                var myString = objA.Content.Split(new char[] { ',' });
                                bytes = Convert.FromBase64String(myString[1]);
                            }
                            else
                                bytes = Convert.FromBase64String(objA.Content);

                            if (objA.FileName.Length > 0 && bytes.Length > 0)
                            {
                                string filePath = HttpContext.Current.Server.MapPath("/Attachments/ResponseAttachment/" + objOutput.SRId.StakeholderResponseId + "_" + objA.FileName);
                                File.WriteAllBytes(filePath, bytes);
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
                #endregion
            }
            return Ok();

        }

        [HttpGet]
        [ActionName("Download")]
        public HttpResponseMessage DownloadNotification(Int64 Id)
        {
            NotificationBusinessService objBS = new NotificationBusinessService();
            MemoryStream mStream = null;
            objBS.GenerateNotificationZip(Id, out mStream);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(mStream.GetBuffer())
            };
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = "NotificationDetails.zip"
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = mStream.Length;

            if (mStream != null)
            {
                mStream.Close();
                mStream.Dispose();
            }

            return result;
        }

        [HttpGet]
        public IHttpActionResult EditAction(Int64 Id)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            return Ok(objAN.EditNotificationAction(Id));
        }

        [HttpPost]
        public IHttpActionResult GetMailSMSTemplate(Int64 Id, Notification_Template_Search obj)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            return Ok(objAN.GetNotificationSMSMailTemplate(Id, obj));
        }

        [HttpPost]
        public IHttpActionResult GetStakeHoldersMaster(string SearchText)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            return Ok(objAN.GetStakeHoldersMaster(SearchText));
        }

        [HttpPost]
        public IHttpActionResult ValidateNotification(CheckNotification obj)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            return Ok(objAN.ValidateNotification(obj));
        }

        [HttpGet]
        public IHttpActionResult NotificationDocuments(Int64 Id)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            return Ok(objAN.GetNotificationDocuments(Id));
        }

        [HttpPost]
        public IHttpActionResult SendMailforAction(long Id, ActionMailDetails MailDetails)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            SendActionMail_Output objOutput = objAN.SaveAndSendMailforAction(Id, MailDetails);

            if (objOutput != null)
            {
                #region "Attachments"
                if (MailDetails.Attachments != null && MailDetails.Attachments.Count > 0 && objOutput.Attachments != null)
                {
                    foreach (Attachement_Action objAttach in MailDetails.Attachments)
                    {
                        foreach (EditAttachment att in objOutput.Attachments)
                        {
                            if (objAttach.Content != "" && att.FileName == objAttach.FileName)
                            {
                                try
                                {
                                    byte[] bytes = null;
                                    if (objAttach.Content.IndexOf(',') >= 0)
                                    {
                                        var myString = objAttach.Content.Split(new char[] { ',' });
                                        bytes = Convert.FromBase64String(myString[1]);
                                    }
                                    else
                                        bytes = Convert.FromBase64String(objAttach.Content);

                                    if (objAttach.FileName.Length > 0 && bytes.Length > 0)
                                    {
                                        string filePath = HttpContext.Current.Server.MapPath(att.Path);
                                        File.WriteAllBytes(filePath, bytes);
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                }
                #endregion

                SendMail objMail = new SendMail();
                List<string> Attachment = new List<string>();

                foreach (EditAttachment objAttach in objOutput.Attachments)
                {
                    Attachment.Add(HttpContext.Current.Server.MapPath(objAttach.Path));
                }
                objMail.SendAsyncEMail(objOutput.MailTo, objOutput.CC, objOutput.BCC, objOutput.ReplyTo, objOutput.DisplayName, objOutput.Subject, objOutput.Body, Attachment.ToArray());
            }

            return Ok();
        }

        [HttpGet]
        public IHttpActionResult ViewAction(Int64 Id)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            return Ok(objAN.ViewAction(Id));
        }

        [HttpGet]
        public IHttpActionResult NotificationDate(Int64 Id, string DateOfNotification, string FinalDateOfComments)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            return Ok(objAN.getCalculatedDate(Id, DateOfNotification, FinalDateOfComments));
        }
        [HttpGet]
        public IHttpActionResult GetMailDetailsSendtoStakeholder(string MailId, string callFor)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            return Ok(objAN.GetMailDetailsSendtoStakeholder(MailId, callFor));
        }
        [HttpGet]
        public IHttpActionResult ViewResponseFromStackHolder(Int64 Id)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            return Ok(objAN.ViewResponseFromStackHolder(Id));
        }

        [HttpPost]
        public IHttpActionResult SaveNote(SaveNote obj)
        {
            NotificationBusinessService objBS = new NotificationBusinessService();
            return Ok(objBS.UpdateMeetingNote(obj));
        }

        [HttpGet]
        public IHttpActionResult GetMeetingNotes(int Id)
        {
            NotificationBusinessService objBS = new NotificationBusinessService();
            return Ok(objBS.GetMeetingNotes(Id));
        }
    }
}
