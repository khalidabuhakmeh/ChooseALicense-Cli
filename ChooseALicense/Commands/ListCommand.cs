using System.ComponentModel;
using ChooseALicense.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using License = ChooseALicense.Models.License;

#pragma warning disable CS8765

namespace ChooseALicense.Commands;

public class ListCommand : Command<ListCommand.Settings>
{
    private readonly LicenseInformation _licenseInformation;

    public class Settings : CommandSettings
    {
        [CommandOption("-l|--limit")]
        [Description("limit the number of results returned")]
        public int? Limit { get; set; }
        [CommandOption("-s|--search")]
        [Description("search id/title for a specific license")]
        public string? Search { get; set; }
    }
    
    public ListCommand(LicenseInformation licenseInformation)
    {
        _licenseInformation = licenseInformation;
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var limit = settings.Limit ?? 100;
        var table = new Table()
            .Title("Software Licenses")
            .RoundedBorder()
            .AddColumns(
                nameof(License.Id),
                nameof(License.Title),
                nameof(License.Permissions),
                nameof(License.Conditions),
                nameof(License.Limitations)
            )
            .ShowFooters();

        IEnumerable<License> licenses = _licenseInformation.Licenses.All;

        if (settings.Search is { })
        {
            licenses = licenses.Where(l =>
                l.Title.Contains(settings.Search, StringComparison.InvariantCultureIgnoreCase) ||
                l.Id.Contains(settings.Search, StringComparison.InvariantCultureIgnoreCase)
            );
        }

        var rules = _licenseInformation.Rules;
        foreach (var license in licenses.Take(limit))
        {
            table.AddRow(
                $"[green]{license.Id}[/]",
                $"[green]{license.Title}[/]",
                rules.Permissions.ToLabelBullets(license.Permissions),
                rules.Conditions.ToLabelBullets(license.Conditions),
                rules.Limitations.ToLabelBullets(license.Limitations)
            );
        }

        table.Caption(settings.Search is { } s ? $"Searched: {s}" : "All Licenses");
        AnsiConsole.Write(table);
        return 0;
    }
}