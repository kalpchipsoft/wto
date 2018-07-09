using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    public class GlobalErrorModel
    {
       
        public string Detail { get; set; }
        public string Subject { get; set; }
        public string RefrenceNo { get; set; }
        public string SentBy { get; set; }
    }

    public class  GlobalResult : CommonResponseModel
    {
        public string Status { get; set; }
    }
}