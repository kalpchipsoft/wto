using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Notification
{
    public class Dashboard
    {
        [Required(ErrorMessage = "UserId missing in session.")]
        [Range(1, Int64.MaxValue)]
        public Int64 UserId { get; set; }
    }

    public class Result_DiscussionCounts : CommonResponseModel
    {
        public long PendingDoc { get; set; }

        public long PendingTrans { get; set; }

        public long ToSendToStakeholder { get; set; }

        public long PendingDiscuss { get; set; }
    }

    public class Result_ActionsCounts : CommonResponseModel
    {
        public ActionCountsType Response { get; set; }

        public ActionCountsType BriefToReg { get; set; }

        public ActionCountsType BriefToDoc { get; set; }
    }

    public class Result
    {
        public Result_DiscussionCounts Discussion { get; set; }

        public Result_ActionsCounts Actions { get; set; }
    }

    public class ActionCountsType
    {
        public long OverDue { get; set; }

        public long Total { get; set; }
    }
}
