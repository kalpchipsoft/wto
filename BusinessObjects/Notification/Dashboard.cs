using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Notification
{
    public class Dashboard
    {
        [Required(ErrorMessage = "UserId missing in session.")]
        [Range(1, Int64.MaxValue)]
        public Int64 UserId { get; set; }
    }

    public class Count_Discussion: StatusMaster
    {
        public int Total { get; set; }
        public string CssColor { get; set; }
        public string Icon { get; set; }
    }

    public class Count_Action: ActionMaster
    {
        public int Total { get; set; }
        public int OverDue { get; set; }
        public string CssColor { get; set; }
    }

    public class Dashboard_PendingCounts
    {
        public List<Count_Discussion> PendingDiscussions { get; set; }

        public List<Count_Action> PendingActions { get; set; }
    }
}
