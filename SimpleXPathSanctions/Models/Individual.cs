
namespace SimpleXPathSanctions.Models
{
    public class Individual
    {
        public int DataId { get; set; }
        public int VersionNumber { get; set; }
        public string? FirstName{ get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string? UnitedNationListType { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? ListedOn { get; set; }
        public string? OriginalScript { get; set; }
        public string? Comments { get; set; }
        public string? Title { get; set; }
        public List<Designation>? Designations { get; set; } //plural name, more than one, so make it a list
        public string? Nationality { get; set; }
        public string? ListType { get; set; }
        public List<LastUpdate>? Updates { get; set; } //plural name, friendly-name, more than one, so make it a list
        public List<Alias>? Aliases { get; set; }
        public List<Address>? Addresses { get; set; }
        public DateOfBirth? DateOfBirth { get; set; } // field occurs once, but it consist of multiple fields, so use a custom class, instead of just a string
        public PlaceOfBirth? PlaceOfBirth { get; set; } // // field occurs once, but it consist of multiple fields, so use a custom class, instead of just a string
        public List<Document>? Documents { get; set; } //plural name. there can be more than one INDIVIDUAL_DOCUMENT, so make it a list
    }
}
