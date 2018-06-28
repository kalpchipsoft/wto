namespace BusinessObjects.Masters
{
    public class MastersBussinessObject
    {
    }

    public class StakeHolderMaster
    {
        public long ItemNumber { get; set; }
        public long StakeHolderId { get; set; }
        public string FullName { get; set; }
        public string HSCodes { get; set; }
        public string OrganizationName { get; set; }
        public string Designation { get; set; }
    }

    public class HSCodes
    {
        public string HSCode { get; set; }
        public string Text { get; set; }
    }

    public class Country
    {
        public long CountryId { get; set; }
        public string CountryCode { get; set; }
        public string Name { get; set; }
    }

    public class NotificationStatus
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class Language
    {
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class Result
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public long Id { get; set; }
    }

    public class InternalStakeHolderMaster
    {
        public int InternalStakeHolderId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string OrganisationName { get; set; }
        public string Designation { get; set; }
    }

    public class RegulatoryBodiesMaster
    {
        public int RegulatoryBodyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
    }
}
