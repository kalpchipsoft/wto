namespace BusinessObjects
{
    public sealed class Procedures
    {
        public const string Login_Validate = "proc_ValidateUser";
        public const string SaveGlobalErrorDetail = "proc_SaveGlobalErrorDetail";

        #region "Masters"
        public const string GetHSCodeMaster = "proc_GetHSCodeMaster";
        public const string GetLanguageMaster = "proc_GetLanguageMaster";
        public const string CheckIsEmailExists = "proc_IsEmailExists";

        public const string GetInternalStakeholderMaster = "proc_getInternalStakeholders";
        public const string GetRegulatoryBodiesMaster = "proc_getRegulatoryBodies";
        public const string GetHSCodeMaster_AutoComplete = "Proc_GetHSCodeMaster_AutoComplete";
        #endregion

        #region "Dashboard"
        public const string Dashboards_NotificationCountByHsCode = "Proc_GetNotificationCount_ByHsCode";
        public const string Dashboards_NotificationCountByCountry = "Proc_GetNotificationCount_ByCountry";
        public const string Dashboards_NotificationCountByAction = "proc_GetDashboardPendingCounts_Action";
        public const string Dashboards_NotificationCountRequestResponse = "Proc_GetNotificationCountRequestResponse";
        public const string Dashboards_PendingCounts = "proc_GetDashboardPendingCounts";
        public const string Dashboards_NotificationGraphData = "Proc_GetNotificationGraphData";
        public const string Dashboards_PendingCounts_Discussion = "proc_GetDashboardPendingCounts_Discussion";

        public const string Dashboards_NotificationGraphDataWeekly = "Proc_GetNotificationGraphDataWeekly";
        public const string Dashboards_NotificationGraphDataMonthly = "Proc_GetNotificationGraphDataMonthly";
        #endregion

        #region "Add Notification"

        //Add/Update
        public const string Notification_AddUpdate = "proc_InsertUpdateNotification";
        public const string Notification_Edit = "proc_GetPageLoad_NotificationDetails";

        //Stakeholders
        public const string Notification_AddRemoveRelatedStakeholders = "proc_InsertUpdateNotificationRelatedStakeholders";
        public const string GetStakeholderMaster = "proc_GetStakeholderMaster";
        public const string Notification_RelatedStakeholders = "proc_GetNotificationRelatedStakeholders";
        public const string Notification_SendMailToStakeholders = "proc_SendMailsToStakeholders";
        public const string Notification_Mails = "proc_GetMails_Notification";
        public const string Notification_GetStakeHolderConversation = "proc_GetStakeHolderConversation";
        public const string SaveStakeholderResponse = "proc_SaveStakeholderResponse";

        //full text of document
        public const string Notification_Validate = "proc_Validate_Notification";
        public const string Notification_SendMailToEnquiryDesk = "proc_SendMailsToEnquiryDesk";
        public const string Notification_SendToTranslate = "proc_SendDocumentForTranslation";


        //Details
        public const string Notification_GetNotificationDetails = "proc_GetNotificationDetails";
        public const string Notification_MailSendToStakeHolder = "Proc_GetMailDetailSentToStakeHolder";
        public const string Notification_ViewResponseDetails = "proc_NotificationViewResponseDetails";

        //Actions
        public const string Notification_Actions = "proc_GetNotificationActions";
        public const string Notification_EditAction = "proc_EditNotificationAction";
        public const string Notification_SendActionMail = "proc_SendActionMail";
        public const string Notification_ViewActionDetails = "proc_GetNotificationActionDetails";
        public const string Notification_SaveResponseActionMail = "Proc_SaveResponseActionMail";

        //Mail Templates
        public const string Notification_GetTemplate = "proc_GetTemplates_Notification";
        public const string Notification_GetNotificationRelatedDocuments = "proc_GetNotificationRelatedDocuments";

        //Dates Calculations
        public const string Notification_getCalculatedDate = "proc_CalculateNotificationDates";
        public const string GetMeetingNote = "proc_GetMeetingNote";
        #endregion

        #region "Notification List"
        public const string GetNotificationList_PageLoadData = "proc_PageLoadDataNotificationList";
        public const string GetNotificationsList = "proc_GetNotificationList";
        public const string ExportNotificationList = "proc_ExportNotificationList";
        public const string CheckNotificationExistMOM = "Proc_CheckNotificationExistMOM";
        #endregion

        #region "View notification"
        public const string ViewNotification = "proc_GetNotificationDetails_View";
        public const string NotificationSummary = "proc_NotificationSummary";
        public const string InsertUpdateNote = "proc_UpdateMeetingNote";
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
        public const string GetDupCountryCode = "proc_GetDupCountryCode";
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

        #region "Meeting"
        public const string GetNotificationListForMom = "Proc_GetNotificationList_Mom";
        public const string InsertMeeting = "Proc_InsertNotificationMom";
        public const string GetMeetingList = "Proc_GetNotificationMOMList";
        public const string EditMeeting = "Proc_GetEditNotificationMeeting";
        public const string UpdateMeetingDate = "Proc_UpdateMeetingDate";

        public const string AddUpdateNotificationAction = "proc_AddRemoveNotificationAction";
        
        public const string EndMeeting = "proc_EndCurrentMeeting";
        public const string CheckIfOpenMeetingExists = "proc_CheckIfOpenMeetingExists";
        public const string MeetingSummary = "proc_MeetingSummary";
        #endregion

        #region "Translator"
        public const string Validate_Translator = "proc_Validate_Translator";
        public const string Login_Translator = "proc_Login_Translator";
        public const string UpdatePassword_Translator = "proc_UpdatePassword_Translator";
        public const string GetDocumentList_Translator = "proc_GetNotificationList_Translator";
        public const string UploadTranslatedDocument_Translator = "proc_UploadTranslatedDocument_Translator";
        #endregion

        #region "Internal StackHolders"
        public const string GetInternalStackHoldersList = "proc_GetInternalStackHolderList";
        public const string AddInternalStackHolder = "proc_InsertUpdateInternalStackHolder";
        public const string GetInternalStackHolderDetails = "proc_GetInternalStackHolderList";
        public const string DeleteInternalStackHolder = "proc_DeleteInternalStackHolder";
        #endregion

        #region "Regulatory Bodies"
        public const string GetRegulatoryBodiesList = "proc_GetRegulatoryBodiesList";
        public const string AddRegulatoryBodies = "proc_InsertUpdateRegulatoryBodies";
        public const string GetRegulatoryBodiesDetails = "proc_GetRegulatoryBodiesList";
        public const string DeleteRegulatoryBodies = "proc_DeleteRegulatoryBodies";
        #endregion

        #region "Language Details"
        public const string GetLanguageList = "proc_GetLanguages";
        public const string AddLanguage = "proc_InsertUpdateLanguage";
        public const string GetLanguageDetails = "proc_GetLanguages";
        public const string DeleteLanguage = "proc_DeleteLanguage";
        public const string DuplicateLanguage = "proc_CheckDuplicateLanguage";
        #endregion

        #region "Notification Country List"
        public const string GetPageLoadCountriesNotificationList = "Proc_GetPageLoadCountriesNotificationCount";
        public const string GetCountriesNotificationList = "Proc_GetCountriesNotificationCount";
        #endregion

        #region "Stakholder Mail Sent and Response List"
        public const string GetStakholdersMailSentResponseList = "Proc_GetStakholdersMailSentResponseCount";
        #endregion
    }
}
