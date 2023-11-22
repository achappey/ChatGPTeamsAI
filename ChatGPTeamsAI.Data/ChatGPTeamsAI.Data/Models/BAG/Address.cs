using ChatGPTeamsAI.Data.Attributes;
using CsvHelper.Configuration.Attributes;

namespace ChatGPTeamsAI.Data.Models.BAG;

internal class Address
{
    [FormColumn]
    public string? OpenbareRuimteNaam { get; set; }

    [FormColumn]
    [ListColumn]
    public string? KorteNaam { get; set; }

    [ListColumn]
    [FormColumn]
    public string? Postcode { get; set; }

    [ListColumn]
    [FormColumn]
    public int Huisnummer { get; set; }

    [FormColumn]
    public string? WoonplaatsNaam { get; set; }

    [FormColumn]
    public double? Latitude
    {
        get
        {
            return AdresseerbaarObjectGeometrie?.Punt?.Latitude;
        }
        set { }
    }

    [FormColumn]
    public double? Longitude
    {
        get
        {
            return AdresseerbaarObjectGeometrie?.Punt?.Longitude;
        }
        set { }
    }

    [FormColumn]
    public string? NummeraanduidingIdentificatie { get; set; }

    [FormColumn]
    public string? OpenbareRuimteIdentificatie { get; set; }

    [FormColumn]
    public string? WoonplaatsIdentificatie { get; set; }

    [FormColumn]
    public string? AdresseerbaarObjectIdentificatie { get; set; }

    [Ignore]
    public List<string>? PandIdentificaties { get; set; }

    [FormColumn]
    public string? Adresregel5 { get; set; }

    [FormColumn]
    public string? Adresregel6 { get; set; }

    [FormColumn]
    public string? TypeAdresseerbaarObject { get; set; }

    [Ignore]
    public AdresseerbaarObjectGeometry? AdresseerbaarObjectGeometrie { get; set; }

    [FormColumn]
    public string? AdresseerbaarObjectStatus { get; set; }

    [Ignore]
    public List<string>? Gebruiksdoelen { get; set; }

    [FormColumn]
    public int? Oppervlakte { get; set; }

    [Ignore]
    public List<string>? OorspronkelijkBouwjaar { get; set; }

    [Ignore]
    public List<string>? PandStatussen { get; set; }

    [Ignore]
    public Links? Links { get; set; }
}

internal class AdresseerbaarObjectGeometry
{
    public Point? Punt { get; set; }
}

internal class Point
{
    public string? Type { get; set; }
    public List<double>? Coordinates { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

internal class Links
{
    public Self? Self { get; set; }
    public Self? OpenbareRuimte { get; set; }
    public Self? Nummeraanduiding { get; set; }
    public Self? Woonplaats { get; set; }
    public Self? AdresseerbaarObject { get; set; }
    public List<Building>? Panden { get; set; }
}
