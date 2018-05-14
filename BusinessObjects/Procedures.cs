namespace BusinessObjects
{
    public sealed class Procedures
    {
        public const string Login_Validate = "proc_ValidateUser";

        #region "Masters"
        public const string GetHSCodeMaster = "proc_GetHSCodeMaster";
        public const string GetLanguageMaster = "proc_GetLanguageMaster";
        public const string CheckIsEmailExists = "proc_IsEmailExists";
        #endregion

        #region "Dashboard"
        public const string Dashboards_PendingCounts = "proc_GetDashboardPendingCounts";
        #endregion

        #region "Add Notification"
        public const string Notification_AddUpdate = "proc_InsertUpdateNotification";
        public const string Notification_Edit = "proc_GetPageLoad_NotificationDetails";
        public const string Notification_AddRemoveRelatedStakeholders= "proc_InsertUpdateNotificationRelatedStakeholders";
        public const string Notification_RelatedStakeholders = "proc_GetNotificationRelatedStakeholders";
        public const string Notification_SendToTranslate = "proc_SendDocumentForTranslation";
        public const string Notification_SendMailToStakeholders = "proc_SendMailsToStakeholders";
        public const string Notification_SendMailToEnquiryDesk = "proc_SendMailsToEnquiryDesk";
        public const string Notification_GetNotificationDetails = "proc_GetNotificationDetails";

        public const string Notification_Actions = "proc_GetNotificationActions";
        public const string Notification_InsertUpdateAction = "proc_InsertUpdateNotificationAction";
        public const string Notification_GetTemplate = "proc_GetTemplates_Notification";
        public const string Notification_Mails = "proc_GetMails_Notification";

        public const string Notification_GetStakeHolderConversation = "proc_GetStakeHolderConversation";
        #endregion

        #region "Notification List"
        public const string GetNotificationList_PageLoadData= "proc_PageLoadDataNotificationList";
        public const string GetNotificationsList = "proc_GetNotificationList";
        #endregion

        #region "Manage Access"
        
        #region "Users"
        public const string GetUsersList = "proc_GetUsersList";
        public const string AddUser = "proc_InsertUpdateUser";
        public const string GetUserDetails = "proc_GetUsersList";
        public const string DeleteUser = "proc_DeleteUser";
        #endregion

        #region "Countries"
        public const string GetCountriesList = "proc_GetCountryList";
        public const string AddCountry = "proc_InsertUpdateCountry";
        public const string GetCountryDetails = "proc_GetCountryList";
        public const string DeleteCountry = "proc_DeleteCountry";
        #endregion

        #region "StakeHolders"
        public const string GetStakeHoldersList = "proc_GetStakeholderList";
        public const string AddStakeHolder = "proc_InsertUpdateStakeholder";
        public const string GetStakeHolderDetails = "proc_GetStakeholderList";
        public const string DeleteStakeHolder = "proc_DeleteStakeholder";
        #endregion

        #region "Translators"
        public const string GetTranslatorsList = "proc_GetTranslatorList";
        public const string AddTranslator = "proc_InsertUpdateTranslator";
        public const string GetTranslatorDetails = "proc_GetTranslatorList";
        public const string DeleteTranslator = "proc_DeleteTranslator";
        public const string SendMailToTranslator = "proc_SendWelcomeMailToTranslater";
        #endregion

        #region "SMS /Mail Template"
        public const string MailFields = "proc_GetFieldsforTemplate";
        public const string GetTemplatesList = "proc_GetTemplates";
        public const string InsertUpdateTemplate = "proc_InsertUpdateTemplate";
        public const string GetTemplateDetails = "proc_GetTemplates";
        public const string DeleteTemplate = "proc_DeleteTemplate";
        #endregion

        #endregion

        #region "Add MOM"
        public const string GetNotificationListForMom = "Proc_GetNotificationList_Mom";
        public const string InsertUpdateMom = "Proc_InsertUpdateNotificationMom";
        public const string GetMOMListData = "Proc_GetNotificationMOMList";
        #endregion

        #region "Translator"
        public const string Validate_Translator = "proc_Validate_Translator";
        public const string Login_Translator = "proc_Login_Translator";
        public const string UpdatePassword_Translator = "proc_UpdatePassword_Translator";
        public const string GetDocumentList_Translator = "proc_GetNotificationList_Translator";
        public const string UploadTranslatedDocument_Translator = "proc_UploadTranslatedDocument_Translator";
        #endregion
    }
}
