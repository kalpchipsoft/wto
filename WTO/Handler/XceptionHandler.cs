using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace WTO.Handler
{
    public class XceptionHandler : ExceptionLogger
    {
        public override async void Log(ExceptionLoggerContext context)
        {
            try
            {
                string strParameter = await Read(context.Request);

                File.AppendAllText(HttpContext.Current.Server.MapPath("~/XceptionLog.txt"),
                    String.Format(Environment.NewLine + "[{0}] - {1}, {2}, {3}, {4}, {5}",
                    DateTime.Now.ToString("dd MMM yyyy HH:mm:ss"),
                    context.Request.Method,
                    context.Request.RequestUri,
                    strParameter,
                    context.Exception.Message,
                    context.Exception.StackTrace.Substring(0, context.Exception.StackTrace.IndexOf("at lambda_method"))
                    ));
            }
            catch { }
        }

        public async Task<string> Read(HttpRequestMessage req)
        {
            using (var contentStream = await req.Content.ReadAsStreamAsync())
            {
                contentStream.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(contentStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}