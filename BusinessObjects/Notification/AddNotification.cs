using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Notification
{
    public class AddNotification
    {
        public long UserId { get; set; }
        public long NotificationId { get; set; }
        public string Noti_Type { get; set; }
        public string Noti_Number { get; set; }
        public Int32 Noti_Status { get; set; }
        public DateTime Noti_Date { get; set; }
        public DateTime FinalDateOfComments { get; set; }
        public DateTime SendResponseBy { get; set; }
        public Int32 Noti_Country { get; set; }
        public string Title { get; set; }
        public string ResponsibleAgency { get; set; }
        public string Noti_UnderArticle { get; set; }
        public string ProductsCovered { get; set; }
        public string HSCodes { get; set; }
        public string Description { get; set; }
    }

    public class AddNoti_Result : CommonResponseModel
    {

    }

    public class EditNotification : AddNotification
    {
        public List<Noti_Type> Noti_TypeList { get; set; }
        public List<Country> CountryList { get; set; }
        public List<HSCodes> SelectedHSCodes { get; set; }
    }

    public class HSCodes
    {
        public string HSCode { get; set; }
        public string Text { get; set; }
    }

    public class Country
    {
        public long CountryId { get; set; }
        public string Name { get; set; }
    }

    public class Noti_Type
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
