using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.Azure.Maps;

internal class SearchAddress
{

    [ListColumn]
    [FormColumn("Point of Interest")]
    public string? Name
    {
        get
        {
            return PointOfInterest?.Name;
        }
        set { }
    }

    [FormColumn("Point of Interest")]
    public string? Phone
    {
        get
        {
            return PointOfInterest?.Phone;
        }
        set { }
    }

    [FormColumn("Point of Interest")]
    public string? Categories
    {
        get
        {
            return PointOfInterest?.Categories != null && PointOfInterest.Categories.Any() ? "- " + string.Join("\r- ", PointOfInterest.Categories) : string.Empty;
        }
        set { }
    }


    [ListColumn]
    [FormColumn("Address")]
    public string? FreeformAddress
    {
        get
        {
            return Address?.FreeformAddress;
        }
        set { }
    }

    [FormColumn("Address")]
    public string? StreetName
    {
        get
        {
            return Address?.StreetName;
        }
        set { }
    }

    [FormColumn("Address")]
    public string? StreetNumber
    {
        get
        {
            return Address?.StreetNumber;
        }
        set { }
    }

    [FormColumn("Address")]
    public string? PostalCode
    {
        get
        {
            return Address?.PostalCode;
        }
        set { }
    }

    [FormColumn("Address")]
    public string? Municipality
    {
        get
        {
            return Address?.Municipality;
        }
        set { }
    }

    [FormColumn("Address")]
    public string? MunicipalitySubdivision
    {
        get
        {
            return Address?.MunicipalitySubdivision;
        }
        set { }
    }

    [FormColumn("Address")]
    public string? CountrySubdivision
    {
        get
        {
            return Address?.CountrySubdivision;
        }
        set { }
    }

    [FormColumn("Address")]
    public string? Country
    {
        get
        {
            return Address?.Country;
        }
        set { }
    }


    [FormColumn("Position")]
    public double? Longitude
    {
        get
        {
            return Position?.Longitude;
        }
        set { }
    }

    [FormColumn("Position")]
    public double? Latitude
    {
        get
        {
            return Position?.Latitude;
        }
        set { }
    }

    [FormColumn("Position")]
    public double? Altitude
    {
        get
        {
            return Position?.Altitude;
        }
        set { }
    }



    [Ignore]
    public Position? Position { get; set; }

    [Ignore]
    public Address? Address { get; set; }

    [Ignore]
    public PointOfInterest? PointOfInterest { get; set; }
}

internal class PointOfInterest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public IEnumerable<string>? Categories { get; set; }
}

internal class Position
{
    public required string DisplayName { get; set; }
    public double? Altitude { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }

}

internal class Address
{
    public string? FreeformAddress { get; set; }
    public string? CountrySubdivision { get; set; }
    public string? Country { get; set; }
    public string? Municipality { get; set; }
    public string? PostalCode { get; set; }
    public string? MunicipalitySubdivision { get; set; }
    public string? StreetName { get; set; }
    public string? StreetNumber { get; set; }
}