using System.Xml;
using System.Linq;
using SimpleXPathSanctions.Models;


var fileName = "consolidated.xml";
var fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Data/", fileName);

var xmlDocument = new XmlDocument();
xmlDocument.Load(fileLocation);

var inputTime = new DateOnly(2015, 07, 05);

// init list
var individuals = new List<Individual>();

// re-used field
DateOnly dateOnly;

var count = 0;

foreach (XmlNode o in xmlDocument.SelectNodes("/CONSOLIDATED_LIST/INDIVIDUALS/INDIVIDUAL")!)
{
    ++count;

    var individual = new Individual();

    // *************************************************************************************
    // fields...
    // *************************************************************************************

    individual.DataId = Convert.ToInt32(o.SelectSingleNode("DATAID")?.InnerText?.Trim());

    individual.VersionNumber = Convert.ToInt32(o.SelectSingleNode("VERSIONNUM")?.InnerText?.Trim());
    individual.FirstName = o.SelectSingleNode("FIRST_NAME")?.InnerText?.Trim();
    individual.SecondName = o.SelectSingleNode("SECOND_NAME")?.Attributes?["name"]?.Value;
    individual.ThirdName = o.SelectSingleNode("THIRD_NAME")?.InnerText?.Trim();
    individual.UnitedNationListType = o.SelectSingleNode("UN_LIST_TYPE")?.InnerText?.Trim();
    individual.ReferenceNumber = o.SelectSingleNode("REFERENCE_NUMBER")?.InnerText?.Trim();
    individual.ListedOn = o.SelectSingleNode("LISTED_ON")?.InnerText?.Trim();
    individual.OriginalScript = o.SelectSingleNode("NAME_ORIGINAL_SCRIPT")?.InnerText?.Trim();
    individual.Comments = o.SelectSingleNode("COMMENTS1")?.InnerText?.Trim();
    individual.Title = o.SelectSingleNode("TITLE/VALUE")?.InnerText?.Trim();

    // *************************************************************************************
    // designations - list
    // *************************************************************************************

    // init list
    individual.Designations = new List<Designation>();

    // loop items to add to list
    foreach (XmlNode des in o.SelectNodes("DESIGNATION/VALUE")!)
    {
        var designation = new Designation();

        designation.Value = des.InnerText?.Trim();

        individual.Designations.Add(designation);
    }

    // *************************************************************************************
    // more fields...
    // *************************************************************************************

    individual.Nationality = o.SelectSingleNode("COMMENTS1")?.InnerText?.Trim();
    individual.ListType = o.SelectSingleNode("TITLE/VALUE")?.InnerText?.Trim();

    // *************************************************************************************
    // last day updated - list
    // *************************************************************************************

    // init list
    individual.Updates = new List<LastUpdate>();

    // loop items to add to list
    foreach (XmlNode ldu in o.SelectNodes("LAST_DAY_UPDATED/VALUE")!)
    {
        var lastUpdate = new LastUpdate();

        // check if not null or empty
        if (DateOnly.TryParse(ldu.InnerText?.Trim()!, out dateOnly))
        {
            lastUpdate.Date = dateOnly;
        }

        individual.Updates.Add(lastUpdate);
    }

    // *************************************************************************************
    // aliases - list
    // *************************************************************************************

    // init list
    individual.Aliases = new List<Alias>();

    // loop items to add to list
    foreach (XmlNode ali in o.SelectNodes("INDIVIDUAL_ALIAS")!)
    {
        var alias = new Alias();

        alias.Quality = ali.SelectSingleNode("QUALITY")?.InnerText?.Trim();
        ;
        alias.AliasName = ali.SelectSingleNode("ALIAS_NAME")?.InnerText?.Trim();
        ;

        individual.Aliases.Add(alias);
    }

    // *************************************************************************************
    // address - list
    // *************************************************************************************

    // loop items to add to list
    foreach (XmlNode adr in o.SelectNodes("./INDIVIDUAL_ADDRESS")!)
    {
        // special check to prevent any items from being added, if its just an empty tag. eg. <INDIVIDUAL_DOCUMENT/>
        if (o.SelectNodes("./INDIVIDUAL_ADDRESS/*")?.Count == 0) { continue; }

        var item = new Address();

        item.Street = adr.SelectSingleNode("STREET")?.InnerText?.Trim();
        item.City = adr.SelectSingleNode("CITY")?.InnerText?.Trim();
        item.StateOrProvince = adr.SelectSingleNode("STATE_PROVINCE")?.InnerText?.Trim();
        item.Country = adr.SelectSingleNode("COUNTRY")?.InnerText?.Trim();
        item.Note = adr.SelectSingleNode("NOTE")?.InnerText?.Trim();

        if (individual.Addresses == null) individual.Addresses = new List<Address>();
        individual.Addresses.Add(item);
    }

    // *************************************************************************************
    // more fields
    // *************************************************************************************

    var dateOfBirth = new DateOfBirth();
    dateOfBirth.TypeOfDate = o.SelectSingleNode("INDIVIDUAL_DATE_OF_BIRTH/TYPE_OF_DATE")?.InnerText?.Trim();

    // check if not null or empty
    if (DateOnly.TryParse(o.SelectSingleNode("INDIVIDUAL_DATE_OF_BIRTH/YEAR")?.InnerText?.Trim(), out dateOnly))
    {
        dateOfBirth.Date = dateOnly; 
    }

    var placeOfBirth = new PlaceOfBirth();
    placeOfBirth.City = o.SelectSingleNode("CITY")?.InnerText?.Trim();
    placeOfBirth.StateOrProvince = o.SelectSingleNode("STATE_PROVINCE")?.InnerText?.Trim();
    placeOfBirth.Country = o.SelectSingleNode("COUNTRY")?.InnerText?.Trim();

    // *************************************************************************************
    // documents - list
    // *************************************************************************************

    // loop items to add to list
    foreach (XmlNode doc in o.SelectNodes("./INDIVIDUAL_DOCUMENT")!)
    {
        // special check to prevent any items from being added, if its just an empty tag. eg. <INDIVIDUAL_DOCUMENT/>
        if (o.SelectNodes("./INDIVIDUAL_DOCUMENT/*")?.Count == 0) { continue; }

        var item = new Document();

        item.TypeOfDocument = doc.SelectSingleNode("TYPE_OF_DOCUMENT")?.InnerText?.Trim();
        item.Number = doc.SelectSingleNode("NUMBER")?.InnerText?.Trim();
        item.IssuingCountry = doc.SelectSingleNode("ISSUING_COUNTRY")?.InnerText?.Trim();
        item.DateOfIssue = doc.SelectSingleNode("DATE_OF_ISSUE")?.InnerText?.Trim();
        item.CityOfIssue = doc.SelectSingleNode("CITY_OF_ISSUE")?.InnerText?.Trim();
        item.CountryOfIssue = doc.SelectSingleNode("COUNTRY_OF_ISSUE")?.InnerText?.Trim();
        item.Note = doc.SelectSingleNode("NOTE")?.InnerText?.Trim();

        if(individual.Documents == null) individual.Documents = new List<Document>();
        individual.Documents.Add(item);
    }

    // *************************************************************************************
    // append to list
    // *************************************************************************************

    // done collection all fields data
    // so add to individual list
    individuals.Add(individual);
}

var sampleQuery =
    from i in individuals
    select new
    {
        i.DataId,
        i.FirstName
    };

var hello = "world";

// doc.SelectNodes("/CONSOLIDATED_LIST/INDIVIDUALS/INDIVIDUAL")?.Cast<Designation>()