using BusinessObjects.ManageAccess;
using System.Collections.Generic;
using System.Linq;

namespace BusinessObjects.Notification
{
    public class RelatedStakeHolders
    {
        public long ItemNumber { get; set; }
        public long StakeHolderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string HSCodes { get; set; }
        public int MailCount { get; set; }
        public int ResponseCount { get; set; }
    }

    public class StackholderMail
    {
        public long StakeholderMailId { get; set; }
        public long MailId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public long StakeholderCount { get; set; }
        public string MailSentDate { get; set; }
    }

    public class StackholderMailDetails : StackholderMail
    {
        public string MailSentTime { get; set; }
        public List<EditAttachment> Attachments { get; set; }
        public string MailType { get; set; }
        public string MailSentBy { get; set; }
    }

    public class SendMailStakeholders : StackholderMail
    {
        public long NotificationId { get; set; }
        public string UserId { get; set; }
        public string Stakeholders { get; set; }
        public List<MailAttachment> Attachments { get; set; }
        public string _Attachments
        {
            get
            {
                if (Attachments == null)
                    return "";
                else
                {
                    return string.Join("|", Attachments.Select(x => x.FileName).ToArray());
                }
            }
        }
    }

    public class MailAttachment
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public string Path { get; set; }
    }

    public class SendMailStakeholders_Output
    {
        public StackholderMail MailDetails { get; set; }

        public List<StakeHolder> StakeHolders { get; set; }
    }
}
