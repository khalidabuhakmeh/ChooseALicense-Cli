namespace ChooseALicense.Models;

public class LicenseInformation
{
    public LicenseInformation(Rules rules, Licenses licenses)
    {
        Rules = rules;
        Licenses = licenses;
    }

    public Rules Rules { get; set; }
    public Licenses Licenses { get; init; }
}