using BusinessObjects.Translator;
using BusinessService.Translator;
using System;
using System.IO;
using System.Web;
using System.Web.Http;

namespace WTO.Controllers.API
{
    public class TranslatorController : ApiController
    {
        [HttpPost]
        public IHttpActionResult ChangePassword(ChangePassword obj)
        {
            TranslatorBusinessService objTBS = new TranslatorBusinessService();
            return Ok(objTBS.ChangePassword(obj));
        }

        [HttpPost]
        public IHttpActionResult SaveTranslatedDocument(long Id, UploadDocument obj)
        {
            TranslatorBusinessService objTBS = new TranslatorBusinessService();
            int res = objTBS.SaveTranslatedDocument(Id, obj);
            if (res > 0)
            {
                #region "Attachments"
                if (obj.Document != null && obj.Document.Content != "")
                {
                    try
                    {
                        byte[] bytes = null;
                        if (obj.Document.Content.IndexOf(',') >= 0)
                        {
                            var myString = obj.Document.Content.Split(new char[] { ',' });
                            bytes = Convert.FromBase64String(myString[1]);
                        }
                        else
                            bytes = Convert.FromBase64String(obj.Document.Content);

                        if (obj.Document.FileName.Length > 0 && bytes.Length > 0)
                        {
                            string filePath = HttpContext.Current.Server.MapPath("/Attachments/NotificationDocument_Translated/" + obj.NotificationId + "_" + obj.Document.FileName);
                            File.WriteAllBytes(filePath, bytes);
                        }
                    }
                    catch (Exception ex) { }
                }
                #endregion
            }
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult SearchDocuments(SearchDocument objS)
        {
            TranslatorBusinessService objTBS = new TranslatorBusinessService();
            return Ok(objTBS.Documents(objS));
        }
    }
}
