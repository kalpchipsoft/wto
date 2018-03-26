using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public sealed class Procedures
    {
        public const string Login_Validate = "proc_ValidateUser";

        #region "Masters"
        public const string GetHSCodeMaster = "proc_GetHSCodeMaster";
        #endregion

        #region "Dashboard"

        public const string Dashboard_PendingDiscussionCount = "proc_GetDashboard_PendingDiscussionCount";

        public const string Dashboard_PendingActionCount = "proc_GetDashboard_PendingActionCount";

        #endregion

        #region "Add Notification"
        public const string Notification_AddUpdate = "proc_InsertUpdateNotification";
        public const string Notification_Edit = "proc_GetPageLoad_NotificationDetails";
        #endregion

        #region "Notification List"
        public const string GetNotificationListMaters = "proc_NotificationListMaters";
        public const string GetNotificationsList = "proc_GetNotificationList";
        #endregion

        #region "Manage Access"
        
        #region "Users"
        public const string GetUsersList = "proc_GetUsersList";
        public const string AddUser = "proc_AddUser";
        public const string GetUserDetails = "proc_GetUsersList";
        public const string DeleteUser = "proc_DeleteUser";
        #endregion

        #region "Countries"
        public const string GetCountriesList = "proc_GetCountryList";
        public const string AddCountry = "proc_AddCountry";
        public const string GetCountryDetails = "proc_GetCountryList";
        public const string DeleteCountry = "proc_DeleteCountry";
        #endregion

        #region "StakeHolders"
        public const string GetStakeHoldersList = "proc_GetStakeholderList";
        public const string AddStakeHolder = "proc_AddStakeholder";
        public const string GetStakeHolderDetails = "proc_GetStakeholderList";
        public const string DeleteStakeHolder = "proc_DeleteStakeholder";
        #endregion

        #region "Translators"
        public const string GetTranslatorsList = "proc_GetTranslatorList";
        public const string AddTranslator = "proc_AddTranslator";
        public const string GetTranslatorDetails = "proc_GetTranslatorList";
        public const string DeleteTranslator = "proc_DeleteTranslator";
        #endregion

        #endregion
    }
}
